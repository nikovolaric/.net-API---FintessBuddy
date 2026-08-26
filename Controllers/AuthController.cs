using System.Net.Mime;
using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fitnessBudyApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IWebHostEnvironment _env;

    public AuthController(IAuthService authSerivice, IWebHostEnvironment env)
    {
        _authService = authSerivice;
        _env = env;
    }

    [HttpPost("signup")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest req)
    {
        if (req.password != req.confirmPassword)
        {
            return BadRequest("Passwords must match!");
        }

        var result = await _authService.SignUpService(req);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        var user = result.Data!;

        return Created("/api/auth", new { user.id, user.username });
    }

    [HttpPost("login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login([FromBody] LoginRequest req)
    {
        var result = await _authService.LoginService(req);

        if (!result.IsSuccess)
        {
            return Unauthorized(new { message = result.Error });
        }

        Response.Cookies.Append(
            "access_token",
            result.Token!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !_env.IsDevelopment(),
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60),
            }
        );

        return Ok(new { message = "Logged in." });
    }

    [Authorize]
    [HttpPost("logout")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");

        return Ok(new { message = "Logged out." });
    }
}
