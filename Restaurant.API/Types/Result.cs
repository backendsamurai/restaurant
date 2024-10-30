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

    public static Result NotFound(string code, string type, string message, string detail) =>
        new(ResultStatus.NotFound, new DetailedError(code, type, message, detail));

    public static Result Error(string code, string type, string message, string detail) =>
        new(ResultStatus.Error, new DetailedError(code, type, message, detail));

    public static Result CriticalError(string code, string type, string message, string detail) =>
         new(ResultStatus.CriticalError, new DetailedError(code, type, message, detail));

    public static Result Unavailable(string code, string type, string message, string detail) =>
        new(ResultStatus.Unavailable, new DetailedError(code, type, message, detail));

    public static Result Unauthorized(string code, string type, string message, string detail) =>
        new(ResultStatus.Unauthorized, new DetailedError(code, type, message, detail));

    public static Result Forbidden(string code, string type, string message, string detail) =>
        new(ResultStatus.Forbidden, new DetailedError(code, type, message, detail));

    public static Result Invalid(string code, string type, string message, string detail) =>
        new(ResultStatus.Invalid, new DetailedError(code, type, message, detail));

    public static Result Conflict(string code, string type, string message, string detail) =>
        new(ResultStatus.Conflict, new DetailedError(code, type, message, detail));

    public static Result NotFound(DetailedError detailedError) =>
        new(ResultStatus.NotFound, detailedError);

    public static Result Error(DetailedError detailedError) =>
        new(ResultStatus.Error, detailedError);

    public static Result CriticalError(DetailedError detailedError) =>
         new(ResultStatus.CriticalError, detailedError);

    public static Result Unavailable(DetailedError detailedError) =>
        new(ResultStatus.Unavailable, detailedError);

    public static Result Unauthorized(DetailedError detailedError) =>
        new(ResultStatus.Unauthorized, detailedError);

    public static Result Forbidden(DetailedError detailedError) =>
        new(ResultStatus.Forbidden, detailedError);

    public static Result Invalid(DetailedError detailedError) =>
        new(ResultStatus.Invalid, detailedError);

    public static Result Conflict(DetailedError detailedError) =>
        new(ResultStatus.Conflict, detailedError);

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

    public static Result<T> Success(T? value) => new(ResultStatus.Ok, value);

    public static Result<T> Created(T? value) => new(ResultStatus.Created, value);

    public static Result<T> NoContent() => new(ResultStatus.NoContent);

    public static Result<T> NotFound(string code, string type, string message, string detail) =>
        new(ResultStatus.NotFound, new DetailedError(code, type, message, detail));

    public static Result<T> Error(string code, string type, string message, string detail) =>
        new(ResultStatus.Error, new DetailedError(code, type, message, detail));

    public static Result<T> CriticalError(string code, string type, string message, string detail) =>
         new(ResultStatus.CriticalError, new DetailedError(code, type, message, detail));

    public static Result<T> Unavailable(string code, string type, string message, string detail) =>
        new(ResultStatus.Unavailable, new DetailedError(code, type, message, detail));

    public static Result<T> Unauthorized(string code, string type, string message, string detail) =>
        new(ResultStatus.Unauthorized, new DetailedError(code, type, message, detail));

    public static Result<T> Forbidden(string code, string type, string message, string detail) =>
        new(ResultStatus.Forbidden, new DetailedError(code, type, message, detail));

    public static Result<T> Invalid(string code, string type, string message, string detail) =>
        new(ResultStatus.Invalid, new DetailedError(code, type, message, detail));

    public static Result<T> Conflict(string code, string type, string message, string detail) =>
        new(ResultStatus.Conflict, new DetailedError(code, type, message, detail));

    public static Result<T> NotFound(DetailedError detailedError) =>
        new(ResultStatus.NotFound, detailedError);

    public static Result<T> Error(DetailedError detailedError) =>
        new(ResultStatus.Error, detailedError);

    public static Result<T> CriticalError(DetailedError detailedError) =>
         new(ResultStatus.CriticalError, detailedError);

    public static Result<T> Unavailable(DetailedError detailedError) =>
        new(ResultStatus.Unavailable, detailedError);

    public static Result<T> Unauthorized(DetailedError detailedError) =>
        new(ResultStatus.Unauthorized, detailedError);

    public static Result<T> Forbidden(DetailedError detailedError) =>
        new(ResultStatus.Forbidden, detailedError);

    public static Result<T> Invalid(DetailedError detailedError) =>
        new(ResultStatus.Invalid, detailedError);

    public static Result<T> Conflict(DetailedError detailedError) =>
        new(ResultStatus.Conflict, detailedError);

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