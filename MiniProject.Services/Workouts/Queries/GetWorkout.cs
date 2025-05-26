using MiniProject.Data;
using MediatR;
using MiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.Workouts.Queries;

public class GetWorkoutDetails
{

    public class Query : IRequest<Workout>
    {
        public required string Id { get; set; }
    }

    public class Handler(FitnessContext context) : IRequestHandler<Query, Workout> 
    {
        public async Task<Workout> Handle(Query request, CancellationToken ct) 
        {
            var Workout = await context.Workouts.FindAsync([request.Id], ct) ?? throw new Exception("Workout not found!");
            return Workout;
        }
    }
}