
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.Workouts.Commands;

public class CreateWorkout
{

    public class Command : IRequest<string>
    {
        public required Workout Workout { get; set ;}
    }


    public class Handler(FitnessContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken ct)
        {
            context.Workouts.Add(request.Workout);
            await context.SaveChangesAsync(ct);
            return request.Workout.Id.ToString();

        }
    }
}