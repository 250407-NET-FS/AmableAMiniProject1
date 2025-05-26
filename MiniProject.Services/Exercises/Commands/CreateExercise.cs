
using MiniProject.Models;
using MiniProject.Data;
using MediatR;

namespace MiniProject.Services.Exercises.Commands;

public class CreateExercise
{

    public class Command : IRequest<string>
    {
        public required Exercise Exercise { get; set ;}
    }


    public class Handler(FitnessContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken ct)
        {
            context.Exercises.Add(request.Exercise);
            await context.SaveChangesAsync(ct);
            return request.Exercise.Id.ToString();

        }
    }
}