
using MiniProject.Models;
using Microsoft.AspNetCore.Mvc;
using MiniProject.Services.WorkoutExercises.Queries;
using MiniProject.Services.WorkoutExercises.Commands;
using System.Security.Claims;

namespace MiniProject.Api.Controllers;


public class WorkoutExercisesController : ApiController
{

    [HttpGet]
    public async Task<ActionResult<List<WorkoutExercise>>> GetWorkoutExercises(CancellationToken ct)
    {
        return await Mediator.Send(new GetWorkoutExerciseList.Query(), ct);
    }

    [HttpGet("{workoutId}/{exerciseId}")]
    public async Task<ActionResult<WorkoutExercise>> GetWorkoutExerciseById(string workoutId, string exerciseId, CancellationToken ct)
    {
        if (!Guid.TryParse(workoutId, out var workoutGuid) || !Guid.TryParse(exerciseId, out var exerciseGuid))
        {

            return BadRequest("Invalid GUID format for workoutId or exerciseId.");
        }
        return await Mediator.Send(new GetWorkoutExerciseDetails.Query { WorkoutId = workoutGuid, ExerciseId = exerciseGuid }, ct);
    }

    [HttpGet("{userid}")]
    public async Task<ActionResult<List<WorkoutExercise>>> GetWorkoutExerciseByUserId(string userid, CancellationToken ct)
    {
        if (!Guid.TryParse(userid, out var userIdGuid))
        {

            return BadRequest("Invalid GUID format for workoutId or exerciseId.");
        }
        return await Mediator.Send(new GetWorkoutExerciseListById.Query { UserId= userIdGuid }, ct);
    }


    [HttpPost]
    public async Task<ActionResult<string>> CreateWorkoutExercise(WorkoutExercise WorkoutExercise, CancellationToken ct)
    {

        return await Mediator.Send(new CreateWorkoutExercise.Command { WorkoutExercise = WorkoutExercise }, ct);
    }

    [HttpPut]
    public async Task<ActionResult> EditWorkoutExercise(WorkoutExercise WorkoutExercise, CancellationToken ct)
    {
        await Mediator.Send(new EditWorkoutExercise.Command { WorkoutExercise = WorkoutExercise }, ct);
        return NoContent();
    }

    [HttpDelete("{workoutId}/{exerciseId}")]
    public async Task<ActionResult> DeleteWorkoutExercise(string workoutId, string exerciseId, CancellationToken ct)
    {

        if (!Guid.TryParse(workoutId, out var workoutGuid) || !Guid.TryParse(exerciseId, out var exerciseGuid))
        {

            return BadRequest("Invalid GUID format for workoutId or exerciseId.");
        }

        await Mediator.Send(new DeleteWorkoutExercise.Command { WorkoutId = workoutGuid, ExerciseId = exerciseGuid }, ct);
        return Ok();
    }
}