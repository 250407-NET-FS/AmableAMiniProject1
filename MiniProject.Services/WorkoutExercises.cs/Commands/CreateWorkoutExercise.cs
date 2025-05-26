
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.WorkoutExercises.Commands;

public class CreateWorkoutExercise
{

    public class Command : IRequest<string>
    {
        public required WorkoutExercise WorkoutExercise { get; set ;}
    }


    public class Handler(FitnessContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken ct)
        {
            context.WorkoutExercises.Add(request.WorkoutExercise);
            await context.SaveChangesAsync(ct);
            return request.WorkoutExercise.ToString();

        }
    }
}