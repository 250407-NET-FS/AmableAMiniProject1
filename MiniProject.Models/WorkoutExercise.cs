using System.Text.Json.Serialization;

namespace MiniProject.Models;
public class WorkoutExercise

{

    public Guid WorkoutId { get; set; }
    [JsonIgnore]
    public Workout? Workout { get; set; }

    public Guid ExerciseId { get; set; }
    [JsonIgnore]
    public Exercise? Exercise { get; set; }


    public int Sets { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }
}