using MiniProject.Data;
using MiniProject.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.Exercises.Queries;

public class GetExerciseList
{
    public class Query : IRequest<List<Exercise>> {}

    public class Handler(FitnessContext context) : IRequestHandler<Query, List<Exercise>>
    {
        public async Task<List<Exercise>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Exercises.ToListAsync(cancellationToken);
        }
    }
}