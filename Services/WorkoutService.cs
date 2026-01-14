using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<AddExierciseToWorkoutResult> AddExierciseToWorkoutService(
        string workoutId,
        string userId,
        CreateWorkoutExerciseRequest req
    )
    {
        var workout = await _db.Workouts.FirstOrDefaultAsync(w =>
            w.id == new Guid(workoutId) && w.userid == new Guid(userId)
        );

        if (workout == null)
            return AddExierciseToWorkoutResult.Fail(
                "Workout not found or does not belong to user.",
                401
            );

        var exercise = await _db.Exercises.FindAsync(req.Exerciseid);

        if (exercise == null)
            return AddExierciseToWorkoutResult.Fail("Exercise not found.", 400);

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

        return AddExierciseToWorkoutResult.Success();
    }
}
