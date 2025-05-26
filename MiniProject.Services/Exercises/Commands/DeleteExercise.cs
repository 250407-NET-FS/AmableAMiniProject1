
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.Exercises.Commands;

public class DeleteExercise
{

    public class Command : IRequest
    {
        public required string Id { get; set; }

    }

    public class Handler(FitnessContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
           var Exercise = await context.Exercises
                .FindAsync([request.Id], cancellationToken)
                    ?? throw new Exception("Cannot find activity"); 
            context.Remove(Exercise);
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}