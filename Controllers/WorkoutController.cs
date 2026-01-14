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
    public async Task<ActionResult> CreateWorkout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userGuid = new Guid(userId!);

        var workout = await _workoutService.CreateWorkoutService(userGuid);

        return Created("api/workouts", workout);
    }

    [Authorize]
    [HttpPut("{id}/addexercise")]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult> AddExcercisesToWorkout(
        [FromRoute] string id,
        [FromBody] CreateWorkoutExerciseRequest req
    )
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var result = await _workoutService.AddExierciseToWorkoutService(id, userId!, req);

        if (!result.IsSuccess)
        {
            return result.Statuscode == 400 ? NotFound(result.Error) : Unauthorized(result.Error);
        }

        return Ok(new { message = "Success." });
    }
}
