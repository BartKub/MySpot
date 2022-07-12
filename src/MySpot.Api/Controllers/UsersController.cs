using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpCommandHandler;
    private readonly ICommandHandler<SignIn> _signInCommandHandler;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
    private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
    private readonly ITokenStorage _tokenStorage;

    private readonly IAuthenticator _authenticator;


    public UsersController(ICommandHandler<SignUp> signUpCommandHandler,
        IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
        IQueryHandler<GetUser, UserDto> getUserHandler, 
        IAuthenticator authenticator, 
        ICommandHandler<SignIn> signInCommandHandler, 
        ITokenStorage tokenStorage)
    {
        _signUpCommandHandler = signUpCommandHandler;
        _getUsersHandler = getUsersHandler;
        _getUserHandler = getUserHandler;
        _authenticator = authenticator;
        _signInCommandHandler = signInCommandHandler;
        _tokenStorage = tokenStorage;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        if (string.IsNullOrWhiteSpace(User.Identity?.Name))
        {
            return NotFound();
        }

        var userId = Guid.Parse(User.Identity?.Name);
        var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _getUsersHandler.HandleAsync(new GetUsers()));

    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _signUpCommandHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
        return CreatedAtAction(nameof(Get), new { userId = command.UserId }, null);
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<JwtDto>> Post(SignIn command)
    {
        await _signInCommandHandler.HandleAsync(command);
        var jwt = _tokenStorage.Get();
        return Ok(jwt);
    }

    [HttpGet("jwt")]
    public ActionResult<JwtDto> GetJwt()
    {
        var jwt = _authenticator.CreateToken(Guid.NewGuid());
        return jwt;
    }
}