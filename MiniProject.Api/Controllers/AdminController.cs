// API/Controllers/UserController.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Models;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.Admin.Queries;
using MiniProject.Services.Admin.Commands;
using MiniProject.Models.DTOs;


namespace MiniProject.Api.Controllers;

[Authorize(Roles = "Admin")]

public class AdminController(IMediator mediator) : ApiController

{
    private readonly IMediator _mediator = mediator;


    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
    {
        var dtos = await _mediator.Send(new GetAllUsersQuery());
        return Ok(dtos);
    }



    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetUserByAdminId([FromRoute] Guid id)
    {
        var dto = await _mediator.Send(new GetUserByIdQuery { Id = id });
        if (dto == null) return NotFound();
        return Ok(dto);
    }


    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUserById([FromRoute] Guid id)
    {
        var success = await _mediator.Send(new DeleteUserCommand { Id = id });
        if (!success) return NotFound();
        return Ok(new { deleted = true });
    }

    


    [HttpPost("ban/{id:guid}")]
    public async Task<ActionResult<UserDTO>> BanUser([FromRoute] Guid id)
    {
        var dto = await _mediator.Send(new BanUserCommand { Id = id });
        if (dto == null) return NotFound();
        return Ok(dto);
    }

 
    [HttpPost("unban/{id:guid}")]
    public async Task<ActionResult<UserDTO>> UnbanUser([FromRoute] Guid id)
    {
        var dto = await _mediator.Send(new UnbanUserCommand { Id = id });
        if (dto == null) return NotFound();
        return Ok(dto);
    }
}

