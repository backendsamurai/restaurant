using Restaurant.API.Entities;

namespace Restaurant.API.Data.Seeders;

public static class EmployeeSeeder
{
    public static User? Manager { get; private set; }

    public static void SetManager(string name, string email, string password)
    {
        Manager = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            PasswordHash = password,
            Role = UserRole.Employee
        };
    }
}
