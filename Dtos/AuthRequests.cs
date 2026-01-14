public class LoginRequest
{
    public required string username { get; set; }
    public required string password { get; set; }
}

public class SignUpRequest
{
    public required string username { get; set; }
    public required string password { get; set; }
    public required string confirmPassword { get; set; }
}
