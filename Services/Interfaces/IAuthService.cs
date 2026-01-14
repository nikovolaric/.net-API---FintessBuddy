using fitnessBudyApi.Models;

public interface IAuthService
{
    Task<User> SignUpService(SignUpRequest req);
    Task<LoginResult> LoginService(LoginRequest req);
}

public class LoginResult
{
    public bool IsSuccess { get; }
    public string? Token { get; }
    public string? Error { get; }

    private LoginResult(bool isSuccess, string? token, string? error)
    {
        IsSuccess = isSuccess;
        Token = token;
        Error = error;
    }

    public static LoginResult Success(string token) => new(true, token, null);

    public static LoginResult Fail(string error) => new(false, null, error);
}
