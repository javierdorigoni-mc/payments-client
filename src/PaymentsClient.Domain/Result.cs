namespace PaymentsClient.Domain;

public class Result<T> where T : class
{
    public bool IsSuccessful { get; init; }
    public bool HasFailed => Error != null && !IsSuccessful;
    public string? Error { get; init; }
    public T? Value { get; init; }

    private Result(T? value, bool isSuccess, string? error)
    {
        Value = value;
        IsSuccessful = isSuccess;
        Error = error;
    }

    public static Result<T> Failure(string error) => new Result<T>(default, false, error);

    public static Result<T> Success(T value) => new Result<T>(value, true, null);
}