
using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using AutoMapper;

namespace MiniProject.Services.Exercises.Commands;

public class EditExercise
{

    public class Command : IRequest
    {
        public required Exercise Exercise { get; set; }

    }

    public class Handler(FitnessContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var Exercise = await context.Exercises
                .FindAsync([request.Exercise.Id], cancellationToken) 
                    ?? throw new Exception("Cannot find Exercise");
        

            mapper.Map(request.Exercise, Exercise);
            
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}