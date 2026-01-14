using fitnessBudyApi.Models;
using Microsoft.EntityFrameworkCore;

public class WorkoutService : IWorkoutService
{
    private readonly AppDbContext _db;

    public WorkoutService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Workout> CreateWorkoutService(Guid userId)
    {
        var user = await _db.Users.FindAsync(userId);

        var workout = new Workout { userid = userId, user = user! };

        await _db.Workouts.AddAsync(workout);

        await _db.SaveChangesAsync();

        return workout;
    }

    public async Task<ServiceResult> AddExierciseToWorkoutService(
        string workoutId,
        string userId,
        CreateWorkoutExerciseRequest req
    )
    {
        var workout = await _db.Workouts.FirstOrDefaultAsync(w =>
            w.id == new Guid(workoutId) && w.userid == new Guid(userId)
        );

        if (workout == null)
            return ServiceResult.Fail("Workout not found or does not belong to user.", 401);

        var exercise = await _db.Exercises.FindAsync(req.Exerciseid);

        if (exercise == null)
            return ServiceResult.Fail("Exercise not found.", 400);

        var workoutExercise = new WorkoutExercise
        {
            workoutid = workout.id,
            Exerciseid = req.Exerciseid,
            sets = req.sets,
            repetitions = req.repetitions,
            Exercise = exercise,
            workout = workout,
        };

        workout.workoutExercises.Add(workoutExercise);

        _db.WorkoutExercises.Add(workoutExercise);

        await _db.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteWorkoutService(string workoutId, string userId)
    {
        var workout = await _db.Workouts.FirstOrDefaultAsync(w =>
            w.id == new Guid(workoutId) && w.userid == new Guid(userId)
        );

        if (workout == null)
            return ServiceResult.Fail("Workout not found or does not belong to user.", 401);

        _db.Workouts.Remove(workout);
        await _db.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<List<WorkoutDto>>> GetMyWorkoutsService(string userId)
    {
        var workouts = await _db
            .Workouts.Where(w => w.userid == new Guid(userId))
            .Select(w => new WorkoutDto
            {
                Id = w.id,
                Date = w.date,
                Exercises = w
                    .workoutExercises.Select(we => new WorkoutExerciseDto
                    {
                        ExerciseName = we.Exercise.name,
                        Id = we.id,
                        Sets = we.sets,
                        Repetitions = we.repetitions,
                    })
                    .ToList(),
            })
            .ToListAsync();

        return ServiceResult<List<WorkoutDto>>.Success(workouts);
    }
}
