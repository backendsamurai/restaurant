using Restaurant.API.Entities.Abstractions;

namespace Restaurant.API.Entities;

public sealed class Employee : Entity
{
    public required User User { get; set; }
    public required EmployeeRole Role { get; set; }
}
