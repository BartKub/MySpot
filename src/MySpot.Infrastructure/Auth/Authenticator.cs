using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MySpot.Infrastructure.Auth
{
    internal sealed class Authenticator: IAuthenticator
    {
        private readonly IClock _clock;
        private readonly string _issuer;
        private readonly TimeSpan _expiry;
        private readonly string _audience;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public Authenticator(IClock clock, IOptions<AuthOptions> options)
        {
            _clock = clock;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
            _signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
                    SecurityAlgorithms.HmacSha256);
        }

        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = _clock.Current();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new(ClaimTypes.Role, role),
               // new("persmission", "") we can add custom claims here
            };

            var expires = now.Add(_expiry);
            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JwtDto {AccessToken = token};
        }
    }
}
