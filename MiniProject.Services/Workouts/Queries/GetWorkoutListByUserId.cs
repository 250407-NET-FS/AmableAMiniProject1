using MiniProject.Data;
using MediatR;
using MiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.Services.Workouts.Queries
{
    public class GetWorkoutsByUserId
    {
        // 1) Change the Query to request a GUID (not a string), and return List<Workout>
        public class Query : IRequest<List<Workout>>
        {
            public required Guid UserId { get; set; }
        }

        // 2) Handler filters by Workout.UserId and includes related entities
        public class Handler : IRequestHandler<Query, List<Workout>>
        {
            private readonly FitnessContext _context;

            public Handler(FitnessContext context)
            {
                _context = context;
            }

            public async Task<List<Workout>> Handle(Query request, CancellationToken cancellationToken)
            {

                return await _context.Workouts
                    .Where(w => w.UserId == request.UserId)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
