using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessBudyApi.Controllers;

[ApiController]
[Route("api/exercises")]
public class ExerciseController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IExerciseService _exerciseService;

    public ExerciseController(AppDbContext db, IExerciseService exeriseService)
    {
        _db = db;
        _exerciseService = exeriseService;
    }

    [Authorize]
    [HttpGet(Name = "GetAllExercises")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllExercisesQuery query)
    {
        IQueryable<Exercise> q = _db.Exercises;

        if (!string.IsNullOrWhiteSpace(query.search))
        {
            var s = query.search.Trim();

            q = q.Where(e =>
                EF.Functions.ILike(e.name, $"%{s}%") || EF.Functions.ILike(e.body_part, $"%{s}%")
            );
        }

        var exercises = await q.ToListAsync();
        return Ok(exercises);
    }

    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetOne([FromRoute] long id)
    {
        var exierciseItem = await _db.Exercises.FindAsync(id);

        if (exierciseItem == null)
        {
            return NotFound();
        }

        return Ok(exierciseItem);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateOne(
        [FromRoute] long id,
        [FromBody] UpdateExerciseRequest req
    )
    {
        var result = await _exerciseService.UpdateExerciseService(id, req);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        return Ok(result.Data);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOne([FromRoute] long id)
    {
        var result = await _exerciseService.DeleteExerciseService(id);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpPost("add")]
    public async Task<IActionResult> AddNewExercise([FromBody] AddExerciseRequest req)
    {
        await _exerciseService.AddExerciseService(req);

        return Ok(new { message = "Created" });
    }
}
