
using MiniProject.Models;
using MiniProject.Data;
using MediatR;
using AutoMapper;

namespace MiniProject.Services.Workouts.Commands;

public class EditWorkout
{

    public class Command : IRequest
    {
        public required Workout Workout { get; set; }

    }

    public class Handler(FitnessContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var workout = await context.Workouts
                .FindAsync([request.Workout.Id], cancellationToken) 
                    ?? throw new Exception("Cannot find workout");
        

            mapper.Map(request.Workout, workout);
            
            await context.SaveChangesAsync(cancellationToken);

        }
    }
}