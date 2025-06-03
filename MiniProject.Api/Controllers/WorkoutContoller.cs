
using MiniProject.Models;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.Workouts.Queries;
using MiniProject.Services.Workouts.Commands;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MiniProject.Api.Controllers;

[Authorize]
public class WorkoutsController : ApiController
{

    [HttpGet]
    public async Task<ActionResult<List<Workout>>> GetWorkouts(CancellationToken ct)
    {
        return await Mediator.Send(new GetWorkoutList.Query(), ct);
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<Workout>> GetWorkoutById(string id, CancellationToken ct)
    {
        return await Mediator.Send(new GetWorkoutDetails.Query { Id = id }, ct);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<List<Workout>>> GetWorkoutsByUserId(Guid userId, CancellationToken ct)
    {
        var workouts = await Mediator.Send(new GetWorkoutsByUserId.Query { UserId = userId }, ct);
        return Ok(workouts);
    }


    [HttpPost]
    public async Task<ActionResult<string>> CreateWorkout(Workout Workout, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Workout.UserId = Guid.Parse(userId!);
        return await Mediator.Send(new CreateWorkout.Command { Workout = Workout }, ct);
    }

    [HttpPut]
    public async Task<ActionResult> EditWorkout(Workout Workout, CancellationToken ct)
    {
        await Mediator.Send(new EditWorkout.Command { Workout = Workout }, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteWorkout(string id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteWorkout.Command { Id = id }, ct);
        return Ok();
    }
}