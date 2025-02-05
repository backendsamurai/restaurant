namespace Restaurant.API.Types;

public class Result : IResult
{
    public int Status { get; private set; }

    public DetailedError? DetailedError { get; private set; }

    public bool IsSuccess => Status is
        ResultStatus.Ok or
        ResultStatus.NoContent or
        ResultStatus.Created;

    public bool IsError => !IsSuccess;

    protected Result(int status) => Status = status;

    protected Result(int status, DetailedError error)
    {
        Status = status;
        DetailedError = error;
    }

    public static Result Success() => new(200);

    public static Result Created() => new(201);

    public static Result<T> Success<T>(T? value) where T : class => new(value);

    public static Result<T> Created<T>(T? value) where T : class => new(ResultStatus.Created, value);

    public static Result NoContent() => new(204);

    public static implicit operator Result(DetailedError detailedError) => new(detailedError.Status, detailedError);

    public object? GetValue()
    {
        if (Status == ResultStatus.NoContent)
            return null;

        return new { Status, Error = DetailedError };
    }

    public DetailedError? GetDetailedError() => DetailedError;
}

public class Result<T> : IResult where T : class
{
    public int Status { get; private set; } = ResultStatus.Ok;

    public T? Value { get; private set; }

    public DetailedError? DetailedError { get; set; }

    public bool IsSuccess => Status is
        ResultStatus.Ok or
        ResultStatus.NoContent or
        ResultStatus.Created;

    public bool IsError => !IsSuccess;

    protected Result(int status) => Status = status;

    public Result(T? value) => Value = value;

    public Result(int status, T? value)
    {
        Status = status;
        Value = value;
    }

    public Result(int status, DetailedError? error)
    {
        Status = status;
        DetailedError = error;
    }

    public static implicit operator T?(Result<T> result) => result.Value;

    public static implicit operator Result<T>(T? value) => new(value);

    public static implicit operator Result<T>(Result result) => new(result.Status, result.DetailedError);

    public static implicit operator Result<T>(DetailedError detailedError) => new(detailedError.Status, detailedError);

    public static Result<T> Success(T? value) => new(ResultStatus.Ok, value);

    public static Result<T> Created(T? value) => new(ResultStatus.Created, value);

    public static Result<T> NoContent() => new(ResultStatus.NoContent);

    public object? GetValue()
    {
        if (Status == ResultStatus.NoContent)
            return null;

        return new
        {
            Status,
            Data = Value,
            Error = DetailedError
        };
    }

    public DetailedError? GetDetailedError() => DetailedError;
}