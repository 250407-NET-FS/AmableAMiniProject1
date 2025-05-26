
using MiniProject.Models;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.Exercises.Queries;
using MiniProject.Services.Exercises.Commands;
using Microsoft.AspNetCore.Authorization;

namespace MiniProject.Api.Controllers;

[Authorize]
public class ExercisesController : ApiController
{

    [HttpGet]
    public async Task<ActionResult<List<Exercise>>> GetExercises(CancellationToken ct)
    {
        return await Mediator.Send(new GetExerciseList.Query(), ct);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> GetExerciseById(string id, CancellationToken ct)
    {
        return await Mediator.Send(new GetExerciseDetails.Query { Id = id }, ct);
    }


    [HttpPost]
    public async Task<ActionResult<string>> CreateExercise(Exercise Exercise, CancellationToken ct)
    {
        
        return await Mediator.Send(new CreateExercise.Command { Exercise = Exercise }, ct);
    }

    [HttpPut]
    public async Task<ActionResult> EditExercise(Exercise Exercise, CancellationToken ct)
    {
        await Mediator.Send(new EditExercise.Command { Exercise = Exercise }, ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExercise(string id, CancellationToken ct)
    {
        await Mediator.Send(new DeleteExercise.Command { Id = id }, ct);
        return Ok();
    }
}