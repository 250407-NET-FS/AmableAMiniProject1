using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.Users.Commands;

namespace MiniProject.Api.Controllers;

public class UsersController : ApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterUser.RegisterCommand command, CancellationToken ct)
    {
        return await Mediator.Send(command, ct);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUser.Command command, CancellationToken ct)
    {
        return await Mediator.Send(command, ct);
    }
}
