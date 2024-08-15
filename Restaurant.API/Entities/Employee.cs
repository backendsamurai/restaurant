namespace Restaurant.API.Entities;

public sealed class Employee : Entity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required EmployeeRole Role { get; set; }
}
