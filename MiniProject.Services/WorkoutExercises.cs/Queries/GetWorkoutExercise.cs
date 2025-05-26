using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.WorkoutExercises.Queries;

public class GetWorkoutExerciseDetails
{

    public class Query : IRequest<WorkoutExercise>
    {
        public required Guid WorkoutId { get; set; }
        public required Guid ExerciseId { get; set; }
    }

    public class Handler(FitnessContext context) : IRequestHandler<Query, WorkoutExercise> 
    {
        public async Task<WorkoutExercise> Handle(Query request, CancellationToken ct) 
        {
            var WorkoutExercise = await context.WorkoutExercises.FindAsync([request.WorkoutId, request.ExerciseId], ct) ?? throw new Exception("WorkoutExercise not found!");
            return WorkoutExercise;
        }
    }
}