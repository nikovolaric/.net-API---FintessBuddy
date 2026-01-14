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

    public ExerciseController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpGet(Name = "GetAllExercises")]
    public async Task<IActionResult> GetAll()
    {
        var exercises = await _db.Exercises.ToListAsync();
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
}
