using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth
{
    public static class Extensions
    {
        private const string OptionsSectionName = "auth";

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthOptions>(configuration.GetSection(OptionsSectionName));
            var options = configuration.GetOptions<AuthOptions>(OptionsSectionName);

            services.AddSingleton<IAuthenticator, Authenticator>();
            services
                .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.Audience = options.Audience;
                    o.IncludeErrorDetails = true; // only for testing and debugging
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = options.Issuer,
                        ClockSkew = TimeSpan.Zero, //do not want additioal time ragne when token can be valid
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)) // key to validate against
                    };
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("is-admin", policy =>
                {
                    policy.RequireRole("admin");
                    //policy.RequireClaim("permission", "get-users") from cstom claim
                });
            });

            return services;
        }
    }
}
