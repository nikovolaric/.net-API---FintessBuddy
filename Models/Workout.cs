namespace fitnessBudyApi.Models;

public class Workout
{
    public Guid id { get; set; }
    public List<WorkoutExercise> workoutExercises { get; set; } = new();
    public DateTime date { get; set; } = DateTime.UtcNow;

    public Guid userid { get; set; }
    public User user { get; set; } = null!;
}
