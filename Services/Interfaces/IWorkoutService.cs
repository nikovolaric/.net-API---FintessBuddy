using fitnessBudyApi.Models;

public interface IWorkoutService
{
    Task<Workout> CreateWorkoutService(Guid userId);
    Task<AddExierciseToWorkoutResult> AddExierciseToWorkoutService(
        string workoutId,
        string userId,
        CreateWorkoutExerciseRequest req
    );
}

public class AddExierciseToWorkoutResult
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public int? Statuscode { get; }

    private AddExierciseToWorkoutResult(bool isSuccess, string? error, int? statuscode)
    {
        IsSuccess = isSuccess;
        Error = error;
        Statuscode = statuscode;
    }

    public static AddExierciseToWorkoutResult Success() => new(true, null, null);

    public static AddExierciseToWorkoutResult Fail(string error, int statuscode) =>
        new(false, error, statuscode);
}
