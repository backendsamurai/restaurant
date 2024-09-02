namespace Restaurant.API.Models.User;

public sealed record LoginUserResponse(
    Guid Id,
    Guid UserId,
    string UserName,
    string UserEmail,
    string? EmployeeRole,
    bool IsVerified,
    string AccessToken
);
