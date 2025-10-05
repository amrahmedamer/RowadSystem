namespace RowadSystem.Shard.Abstractions;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; }
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default!, false, error);
}

public class Result<TValue> : Result
{
    public Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _Value = value;
    }
    private readonly TValue _Value;

    public TValue Value => IsSuccess
        ? _Value
        : default!;

}
