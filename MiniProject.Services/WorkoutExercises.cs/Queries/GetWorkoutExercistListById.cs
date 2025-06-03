using MiniProject.Data;
using MiniProject.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.WorkoutExercises.Queries;

public class GetWorkoutExerciseListById
{
    public class Query : IRequest<List<WorkoutExercise>>
    {
        public required Guid UserId { get; set; }
    }

    public class Handler(FitnessContext context) : IRequestHandler<Query, List<WorkoutExercise>>
    {
        public async Task<List<WorkoutExercise>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.WorkoutExercises
                .Include(we => we.Workout)
                .Where(we => we.Workout.UserId == request.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}
