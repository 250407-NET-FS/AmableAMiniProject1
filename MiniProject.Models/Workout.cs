using System.Text.Json.Serialization;
using MiniProject.Models;

public class Workout
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }

    // M–M to Exercise
    [JsonIgnore]
    public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
}