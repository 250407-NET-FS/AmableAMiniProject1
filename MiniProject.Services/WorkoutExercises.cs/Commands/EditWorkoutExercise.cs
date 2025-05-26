
using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using AutoMapper;

namespace MiniProject.Services.WorkoutExercises.Commands;

public class EditWorkoutExercise
{

    public class Command : IRequest
    {
        public required WorkoutExercise WorkoutExercise { get; set; }

    }

    public class Handler(FitnessContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var WorkoutExercise = await context.WorkoutExercises
                .FindAsync([request.WorkoutExercise.WorkoutId, request.WorkoutExercise.ExerciseId], cancellationToken) 
                    ?? throw new Exception("Cannot find WorkoutExercise");
        

            mapper.Map(request.WorkoutExercise, WorkoutExercise);
            
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}