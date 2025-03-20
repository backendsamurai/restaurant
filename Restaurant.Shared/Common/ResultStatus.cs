namespace Restaurant.Shared.Common;

public static class ResultStatus
{
    public const int Ok = 200;
    public const int Created = 201;
    public const int NoContent = 204;
    public const int NotFound = 404;
    public const int Error = 400;
    public const int Forbidden = 403;
    public const int Unauthorized = 401;
    public const int Invalid = 422;
    public const int Conflict = 409;
    public const int Unavailable = 503;
    public const int CriticalError = 500;
}
