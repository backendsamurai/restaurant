namespace Restaurant.API.Models.Employee;

public sealed record UpdateEmployeeModel(
    string? Name,
    string? Email,
    string? Password,
    string? Role
);