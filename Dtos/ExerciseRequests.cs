public class AddExerciseRequest
{
    public required string name { get; set; }
    public required string description { get; set; }
    public required string body_part { get; set; }
}

public class GetAllExercisesQuery
{
    public string? search { get; set; }
}

public class UpdateExerciseRequest
{
    public string? name { get; set; }
    public string? description { get; set; }
    public string? body_part { get; set; }
}
