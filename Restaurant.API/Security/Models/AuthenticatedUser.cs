using Restaurant.API.Entities;

namespace Restaurant.API.Security.Models;

public sealed class AuthenticatedUser
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public UserRole UserRole { get; set; }
    public string? EmployeeRole { get; set; }
}
