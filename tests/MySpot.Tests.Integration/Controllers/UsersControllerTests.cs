using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Security;
using Shouldly;

namespace MySpot.Tests.Integration.Controllers;

public class UsersControllerTests: ControllerTests, IDisposable
{
    [Fact]
    public async Task post_users_should_return_201_status_code()
    {
        await _testDatabase.Context.Database.MigrateAsync(); // here inser data can be made
        
        var command = new SignUp(Guid.Empty, "test-user@mySpot.io", "test-user", "secret", "Test Doe", "user");
        var response = await Client.PostAsJsonAsync("users", command);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task post_sign_in_return_200()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        await _testDatabase.Context.Database.MigrateAsync(); 
        const string password = "secret";

        var user = new User(Guid.NewGuid(), "test-user@mySpot.io", "test-user", passwordManager.Secure(password),
            "Test Doe", "user", DateTime.Now);
        await _testDatabase.Context.AddAsync(user);
        await _testDatabase.Context.SaveChangesAsync();

        var command = new SignIn(user.Email, password);

        var response = await Client.PostAsJsonAsync("users/sign-in", command);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
        
        //Assert
        jwt.ShouldNotBeNull();
        jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task get_users_me_should_return_ok_200_and_user_dto()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        await _testDatabase.Context.Database.MigrateAsync();
        const string password = "secret";

        var user = new User(Guid.NewGuid(), "test-user@mySpot.io", "test-user", passwordManager.Secure(password),
            "Test Doe", "user", DateTime.Now);
        await _testDatabase.Context.AddAsync(user);
        await _testDatabase.Context.SaveChangesAsync();

        Authorize(user.Id, user.Role);
        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");

        //Assert
        userDto.ShouldNotBeNull();
        userDto.UserId.ShouldBe(user.Id.Value);
    }


    [Fact]
    public async Task get_users_me_should_return_ok_200_and_user_dto_from_userRepository()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        const string password = "secret";

        var user = new User(Guid.NewGuid(), "test-user@mySpot.io", "test-user", passwordManager.Secure(password),
            "Test Doe", "user", DateTime.Now);

        await _userRepository.AddAsync(user);
        
        Authorize(user.Id, user.Role);
        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");

        //Assert
        userDto.ShouldNotBeNull();
        userDto.UserId.ShouldBe(user.Id.Value);
    }

    private IUserRepository _userRepository;
    
    private readonly TestDatabase _testDatabase;

    protected override void ConfigureServices(IServiceCollection services)
    {
        _userRepository = new TestUserRepisotory();
        services.AddSingleton(_userRepository);
    }
    
    public UsersControllerTests(OptionsProvider opt) : base(opt)
    {
        _testDatabase = new TestDatabase();
    }

    public void Dispose()
    {
        _testDatabase.Dispose();
    }
}