using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.Workouts.Queries;

public class GetWorkoutList
{
    public class Query : IRequest<List<Workout>> {}

    public class Handler(FitnessContext context) : IRequestHandler<Query, List<Workout>>
    {
        public async Task<List<Workout>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Workouts.ToListAsync(cancellationToken);
        }
    }
}