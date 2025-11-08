namespace AuthSystem.Shared.Contracts;

public sealed record ApiResponse<T>(bool Success, T? Data, ApiError[]? Errors, string? TraceId)
{
    public static ApiResponse<T> Ok(T data, string? traceId = null)
        => new(true, data, null, traceId);

    public static ApiResponse<T> Created(T data, string? traceId = null)
        => new(true, data, null, traceId);

    public static ApiResponse<T> Failure(ApiError[] errors, string? traceId = null)
        => new(false, default, errors, traceId);
}

public sealed record ApiError(string Code, string Message, string? Target = null);