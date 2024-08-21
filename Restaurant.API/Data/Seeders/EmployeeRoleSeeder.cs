using Restaurant.API.Entities;

namespace Restaurant.API.Data.Seeders;

public static class EmployeeRoleSeeder
{
    public readonly static EmployeeRole[] employeeRoles = [
        new EmployeeRole { Id = Guid.NewGuid(), Name = "Waiter" },
        new EmployeeRole { Id = Guid.NewGuid(), Name = "Manager" }
    ];
}