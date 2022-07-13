using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.Time;

namespace MySpot.Tests.Integration.Controllers;

[Collection("api")]// to run tests one by one
public  abstract class ControllerTests: IClassFixture<OptionsProvider>
{
    private readonly IAuthenticator _authenticator;
    protected HttpClient Client { get; }

    protected JwtDto Authorize(Guid userId, string role)
    {
        var jwt = _authenticator.CreateToken(userId, role);
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt.AccessToken}");
        return jwt;
    }
    
    protected ControllerTests(OptionsProvider opt)
    {
        var authOptions = opt.Get<AuthOptions>("auth");
        _authenticator = new Authenticator(new Clock(), new OptionsWrapper<AuthOptions>(authOptions));
        var app = new MySpotTestApp(ConfigureServices);
        Client = app.Client;
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }
    
}