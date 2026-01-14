using fitnessBudyApi.Models;

public class WorkoutExercise
{
    public Guid id { get; set; }

    public Guid workoutid { get; set; }
    public Workout workout { get; set; } = null!;

    public long Exerciseid { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int repetitions { get; set; }
    public int sets { get; set; }
}
