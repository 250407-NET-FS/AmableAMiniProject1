
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.WorkoutExercises.Commands;

public class DeleteWorkoutExercise
{

    public class Command : IRequest
    {
        public required Guid WorkoutId { get; set; }
        public required Guid ExerciseId { get; set; }

    }

    public class Handler(FitnessContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
           var WorkoutExercise = await context.WorkoutExercises
                .FindAsync([request.WorkoutId, request.ExerciseId], cancellationToken)
                    ?? throw new Exception("Cannot find activity"); 
            context.Remove(WorkoutExercise);
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}