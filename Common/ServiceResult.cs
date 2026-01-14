public class ServiceResult
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public int? Statuscode { get; }

    protected ServiceResult(bool isSuccess, string? error, int? statuscode)
    {
        IsSuccess = isSuccess;
        Error = error;
        Statuscode = statuscode;
    }

    public static ServiceResult Success() => new(true, null, null);

    public static ServiceResult Fail(string error, int statuscode) => new(false, error, statuscode);
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; }

    private ServiceResult(bool isSuccess, T? data, string? error, int? statuscode)
        : base(isSuccess, error, statuscode)
    {
        Data = data;
    }

    public static ServiceResult<T> Success(T data) => new(true, data, null, null);

    public static new ServiceResult<T> Fail(string error, int statuscode) =>
        new(false, default, error, statuscode);
}
