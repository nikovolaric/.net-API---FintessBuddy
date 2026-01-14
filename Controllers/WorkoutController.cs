using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fitnessBudyApi.Controllers;

[ApiController]
[Route("api/workouts")]
public class WorkoutController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWorkoutService _workoutService;

    public WorkoutController(AppDbContext db, IWorkoutService workoutService)
    {
        _db = db;
        _workoutService = workoutService;
    }

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> CreateWorkout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userGuid = new Guid(userId!);

        var workout = await _workoutService.CreateWorkoutService(userGuid);

        return Created("api/workouts", workout);
    }

    [Authorize]
    [HttpPut("{id}/addexercise")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> AddExcercisesToWorkout(
        [FromRoute] string id,
        [FromBody] CreateWorkoutExerciseRequest req
    )
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _workoutService.AddExierciseToWorkoutService(id, userId!, req);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        return Ok(new { message = "Success." });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _workoutService.DeleteWorkoutService(id, userId!);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        return NoContent();
    }

    [Authorize]
    [HttpGet("getmy")]
    public async Task<IActionResult> GetMyWorkouts()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _workoutService.GetMyWorkoutsService(userId!);

        return Ok(result.Data);
    }
}
