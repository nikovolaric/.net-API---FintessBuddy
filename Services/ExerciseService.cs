using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ExerciseService : IExerciseService
{
    private readonly AppDbContext _db;

    public ExerciseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ServiceResult> AddExerciseService(AddExerciseRequest req)
    {
        var exercise = new Exercise
        {
            body_part = req.body_part,
            name = req.name,
            description = req.description,
        };

        _db.Exercises.Add(exercise);
        await _db.SaveChangesAsync();

        return ServiceResult.Success();
    }

    public async Task<ServiceResult<Exercise>> UpdateExerciseService(
        long id,
        UpdateExerciseRequest req
    )
    {
        var exercise = await _db.Exercises.FindAsync(id);

        if (exercise == null)
        {
            return ServiceResult<Exercise>.Fail("Exercise does not exists.", 404);
        }

        if (req.body_part is not null)
            exercise.body_part = req.body_part;

        if (req.name is not null)
            exercise.name = req.name;

        if (req.description is not null)
            exercise.description = req.description;

        await _db.SaveChangesAsync();

        return ServiceResult<Exercise>.Success(exercise);
    }

    public async Task<ServiceResult> DeleteExerciseService(long id)
    {
        var exercise = await _db.Exercises.FindAsync(id);

        if (exercise == null)
        {
            return ServiceResult.Fail("Exercise does not exists.", 404);
        }

        _db.Exercises.Remove(exercise);
        await _db.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
