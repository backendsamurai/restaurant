namespace Restaurant.API.Models.Employee;

public sealed record CreateEmployeeModel(
    string? Name,
    string? Email,
    string? Password,
    string? Role
);