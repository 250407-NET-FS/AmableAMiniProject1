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
        var token = await Mediator.Send(command, ct);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return Ok(new { token, message = "Login successful" });
    }
}
