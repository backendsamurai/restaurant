namespace Restaurant.Shared.Common;

public sealed class CommonErrorTypes
{
    public const string ResourceNotFound = "RESOURCE_NOT_FOUND";
    public const string ResourceAlreadyPresent = "RESOURCE_ALREADY_PRESENT";
    public const string ResourceCreatingProblem = "RESOURCE_CREATING_PROBLEM";
    public const string ResourceUpdatingProblem = "RESOURCE_UPDATING_PROBLEM";
    public const string ResourceRemoveProblem = "RESOURCE_REMOVING_PROBLEM";
    public const string InvalidModel = "INVALID_MODEL";
    public const string InvalidQuery = "INVALID_QUERY";
    public const string MissingAuthorization = "MISSING_AUTHORIZATION";
    public const string UnexpectedError = "UNEXPECTED_ERROR";
}