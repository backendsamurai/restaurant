namespace Restaurant.API.Models.Employee;

public sealed record EmployeeResponse(
    Guid EmployeeId,
    Guid UserId,
    string UserName,
    string UserEmail,
    string EmployeeRole,
    bool IsVerified
);