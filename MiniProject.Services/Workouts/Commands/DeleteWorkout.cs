
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.Workouts.Commands;

public class DeleteWorkout
{

    public class Command : IRequest
    {
        public required string Id { get; set; }

    }

    public class Handler(FitnessContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
           var Workout = await context.Workouts
                .FindAsync([request.Id], cancellationToken)
                    ?? throw new Exception("Cannot find activity"); 
            context.Remove(Workout);
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}