public class CreateWorkoutExerciseRequest
{
    public long Exerciseid { get; set; }
    public int repetitions { get; set; }
    public int sets { get; set; }
}

public class WorkoutDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}

public class WorkoutExerciseDto
{
    public Guid Id { get; set; }
    public int Sets { get; set; }
    public int Repetitions { get; set; }
    public string ExerciseName { get; set; } = null!;
}
