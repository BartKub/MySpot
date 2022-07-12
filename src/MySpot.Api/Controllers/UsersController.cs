using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpCommandHandler;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
    private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;

    public UsersController(ICommandHandler<SignUp> signUpCommandHandler,
        IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
        IQueryHandler<GetUser, UserDto> getUserHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
        _getUsersHandler = getUsersHandler;
        _getUserHandler = getUserHandler;
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

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _getUsersHandler.HandleAsync(new GetUsers()));

    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _signUpCommandHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
        return CreatedAtAction(nameof(Get), new { userId = command.UserId }, null);
    }
}