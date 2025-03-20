namespace Restaurant.Shared.Common;

public sealed class DetailedError(int status, string type, string title, string message, ErrorSeverity severity)
{
    public int Status { get; set; } = status;
    public string? Type { get; set; } = type;
    public string? Title { get; set; } = title;
    public string? Message { get; set; } = message;
    public ErrorSeverity Severity { get; set; } = severity;

    public static DetailedError Empty() =>
        new(0, string.Empty, string.Empty, string.Empty, ErrorSeverity.Information);

    public static DetailedError Create(Func<DetailedErrorBuilder, DetailedErrorBuilder> builder) =>
        builder(new DetailedErrorBuilder()).Build();

    public static DetailedError NotFound(string message) =>
        new(ResultStatus.NotFound, CommonErrorTypes.ResourceNotFound, "Resource not found", message, ErrorSeverity.Warning);

    public static DetailedError NotFound(string title, string message) =>
        new(ResultStatus.NotFound, CommonErrorTypes.ResourceNotFound, title, message, ErrorSeverity.Warning);

    public static DetailedError Conflict(string message) =>
        new(ResultStatus.Conflict, CommonErrorTypes.ResourceAlreadyPresent, "Resource already present", message, ErrorSeverity.Warning);

    public static DetailedError Conflict(string title, string message) =>
        new(ResultStatus.Conflict, CommonErrorTypes.ResourceAlreadyPresent, title, message, ErrorSeverity.Warning);

    public static DetailedError Invalid(string message) =>
        new(ResultStatus.Invalid, CommonErrorTypes.InvalidModel, "Invalid provided data", message, ErrorSeverity.Warning);

    public static DetailedError Invalid(string title, string message) =>
        new(ResultStatus.Invalid, CommonErrorTypes.InvalidModel, title, message, ErrorSeverity.Warning);

    public static DetailedError CreatingProblem(string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceCreatingProblem, "Resource creating problem", message, ErrorSeverity.Error);

    public static DetailedError CreatingProblem(string title, string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceCreatingProblem, title, message, ErrorSeverity.Error);

    public static DetailedError UpdatingProblem(string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceUpdatingProblem, "Resource updating problem", message, ErrorSeverity.Error);

    public static DetailedError UpdatingProblem(string title, string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceUpdatingProblem, title, message, ErrorSeverity.Error);

    public static DetailedError RemoveProblem(string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceRemoveProblem, "Resource removing problem", message, ErrorSeverity.Error);

    public static DetailedError RemoveProblem(string title, string message) =>
        new(ResultStatus.Error, CommonErrorTypes.ResourceRemoveProblem, title, message, ErrorSeverity.Error);

    public static DetailedError Unauthorized(string message) =>
        new(ResultStatus.Unauthorized, CommonErrorTypes.MissingAuthorization, "Unauthorized", message, ErrorSeverity.Warning);

    public static DetailedError Unauthorized(string title, string message) =>
        new(ResultStatus.Unauthorized, CommonErrorTypes.MissingAuthorization, title, message, ErrorSeverity.Warning);

    public static DetailedError Unexpected(string message) =>
        new(ResultStatus.Error, CommonErrorTypes.UnexpectedError, "Unexpected error", message, ErrorSeverity.Warning);

    public static DetailedError Unexpected(string title, string message) =>
        new(ResultStatus.Error, CommonErrorTypes.UnexpectedError, title, message, ErrorSeverity.Warning);

    public static DetailedError InvalidQuery(string message) =>
        new(ResultStatus.Error, CommonErrorTypes.InvalidQuery, "InvalidQuery", message, ErrorSeverity.Warning);

    public static DetailedError InvalidQuery(string title, string message) =>
        new(ResultStatus.Error, CommonErrorTypes.InvalidQuery, title, message, ErrorSeverity.Warning);

}
