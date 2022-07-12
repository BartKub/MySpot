using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpCommandHandler;

    public UsersController(ICommandHandler<SignUp> signUpCommandHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
    }

    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _signUpCommandHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
        return NoContent();
    }
}