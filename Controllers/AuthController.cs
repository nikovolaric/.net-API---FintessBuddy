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

    /// <remarks>
    /// User will not actually be created, for database protection. Login credential are provided below.
    /// </remarks>
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

        var user = await _authService.SignUpService(req);

        return Created("/api/auth", new { user.id, user.username });
    }

    /// <remarks>
    /// To login use credentials
    /// username:fitnesspal pw:fitnesspalpw
    /// </remarks>
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
                Secure = _env.IsDevelopment(),
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
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
