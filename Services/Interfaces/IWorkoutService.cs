using fitnessBudyApi.Models;

public interface IWorkoutService
{
    Task<Workout> CreateWorkoutService(Guid userId);
    Task<ServiceResult> AddExierciseToWorkoutService(
        string workoutId,
        string userId,
        CreateWorkoutExerciseRequest req
    );

    Task<ServiceResult> DeleteWorkoutService(string userId, string workoutId);
    Task<ServiceResult<List<WorkoutDto>>> GetMyWorkoutsService(string userId);
}
