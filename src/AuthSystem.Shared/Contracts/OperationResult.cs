namespace AuthSystem.Shared.Contracts;

public sealed class OperationResult<T>
{
    private OperationResult(bool succeeded, T? data, string? error)
    {
        Succeeded = succeeded;
        Data = data;
        Error = error;
    }

    public bool Succeeded { get; }

    public T? Data { get; }

    public string? Error { get; }

    public static OperationResult<T> Success(T data) => new(true, data, null);

    public static OperationResult<T> Failure(string error) => new(false, default, error);
}