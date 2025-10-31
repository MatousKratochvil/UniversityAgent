namespace UniversityAgent.Features.CourseEnrollment.Common;

/// <summary>
/// Represents a result of an operation that can succeed or fail
/// </summary>
/// <typeparam name="T">Type of the value returned on success</typeparam>
public record Result<T>
{
    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? Error { get; }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);

    /// <summary>
    /// Maps the value if successful
    /// </summary>
    public Result<TNew> Map<TNew>(Func<T, TNew> mapper) =>
        IsSuccess && Value is not null
            ? Result<TNew>.Success(mapper(Value))
            : Result<TNew>.Failure(Error ?? "Unknown error");

    /// <summary>
    /// Binds to another result-returning operation
    /// </summary>
    public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> binder) =>
        IsSuccess && Value is not null
            ? binder(Value)
            : Result<TNew>.Failure(Error ?? "Unknown error");

    /// <summary>
    /// Executes an action if successful
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Value is not null)
        {
            action(Value);
        }
        return this;
    }

    /// <summary>
    /// Executes an action if failed
    /// </summary>
    public Result<T> OnFailure(Action<string> action)
    {
        if (IsFailure && Error is not null)
        {
            action(Error);
        }
        return this;
    }
}
