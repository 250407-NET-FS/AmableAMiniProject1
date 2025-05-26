using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.Exercises.Queries;

public class GetExerciseDetails
{

    public class Query : IRequest<Exercise>
    {
        public required string Id { get; set; }
    }

    public class Handler(FitnessContext context) : IRequestHandler<Query, Exercise> 
    {
        public async Task<Exercise> Handle(Query request, CancellationToken ct) 
        {
            var Exercise = await context.Exercises.FindAsync([request.Id], ct) ?? throw new Exception("Exercise not found!");
            return Exercise;
        }
    }
}