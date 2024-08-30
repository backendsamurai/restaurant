namespace Restaurant.API.Models.Employee;

public sealed record LoginEmployeeResponse(
    Guid EmployeeId,
    Guid UserId,
    string UserName,
    string UserEmail,
    bool IsVerified,
    string EmployeeRole,
    string AccessToken
);
