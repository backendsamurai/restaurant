using Restaurant.API.Entities;

namespace Restaurant.API.Data.Seeders;

public static class EmployeeRoleSeeder
{
    public static EmployeeRole[] GetRoles()
    {
        return [
            new EmployeeRole { Id = Guid.NewGuid(), Name = "waiter" },
            new EmployeeRole { Id = Guid.NewGuid(), Name = "manager" }
        ];
    }
}