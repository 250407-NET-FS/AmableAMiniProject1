using MiniProject.Data;
using MiniProject.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.WorkoutExercises.Queries;

public class GetWorkoutExerciseList
{
    public class Query : IRequest<List<WorkoutExercise>> {}

    public class Handler(FitnessContext context) : IRequestHandler<Query, List<WorkoutExercise>>
    {
        public async Task<List<WorkoutExercise>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.WorkoutExercises.ToListAsync(cancellationToken);
        }
    }
}