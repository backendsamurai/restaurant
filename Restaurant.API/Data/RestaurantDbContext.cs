using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Restaurant.API.Data.EntityConfigurations;
using Restaurant.API.Data.Seeders;
using Restaurant.API.Entities;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Services;

namespace Restaurant.API.Data;

public sealed class RestaurantDbContext(
    DbContextOptions options,
    IOptions<ManagerOptions> managerOptions,
    IPasswordHasher passwordHasher
) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLineItem> OrderLineItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(EntityConfigurationsAssembly.GetAssembly());

        var passwordHash = passwordHasher.Hash(managerOptions.Value.Password);

        EmployeeSeeder.SetManager(managerOptions.Value.Name, managerOptions.Value.Email, passwordHash);

        if (EmployeeSeeder.Manager is not null)
        {
            modelBuilder.Entity<User>().HasData(EmployeeSeeder.Manager);
            var managerRole = EmployeeRoleSeeder.employeeRoles.SingleOrDefault(e => e.Name == "manager");

            if (managerRole is not null)
            {
                modelBuilder.Entity<Employee>().HasData(new
                {
                    Id = Guid.NewGuid(),
                    RoleId = managerRole.Id,
                    UserId = EmployeeSeeder.Manager.Id
                });
            }
        }
    }
}
