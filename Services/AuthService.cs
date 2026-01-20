using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using fitnessBudyApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public AuthService(AppDbContext db, IConfiguration configuration, IWebHostEnvironment env)
    {
        _db = db;
        _configuration = configuration;
        _env = env;
    }

    public async Task<User> SignUpService(SignUpRequest req)
    {
        var user = new User
        {
            id = Guid.NewGuid(),
            username = req.username,
            role = Role.user,
        };

        var hasher = new PasswordHasher<User>();
        user.password = hasher.HashPassword(user, req.password);

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return user;
    }

    public async Task<LoginResult> LoginService(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.username == req.username);

        if (user == null || user.password == null)
        {
            return LoginResult.Fail("User not found!");
        }

        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(user, user.password, req.password);

        if (result == 0)
        {
            return LoginResult.Fail("Password is incorrect!");
        }

        var jwtKey = _configuration["JWT_SECRET_KEY"];

        if (string.IsNullOrEmpty(jwtKey))
            throw new Exception("JWT_SECRET missing");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.username),
            new Claim(ClaimTypes.Role, user.role.ToString()),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return LoginResult.Success(tokenString);
    }
}
