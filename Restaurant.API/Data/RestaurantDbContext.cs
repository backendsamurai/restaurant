using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Restaurant.API.Data.EntityConfigurations;
using Restaurant.API.Data.Seeders;
using Restaurant.API.Entities;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Security.Services.Contracts;

namespace Restaurant.API.Data;

public sealed class RestaurantDbContext(
    IConfiguration configuration,
    IOptions<ManagerOptions> managerOptions,
    IPasswordHasherService passwordHasher,
    ILogger<RestaurantDbContext> logger
) : DbContext
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        optionsBuilder
            .UseNpgsql(connectionString)
            .LogTo(msg => logger.LogInformation(msg), LogLevel.Information, DbContextLoggerOptions.UtcTime | DbContextLoggerOptions.SingleLine)
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(EntityConfigurationsAssembly.GetAssembly());

        if (string.IsNullOrEmpty(managerOptions.Value.Name) ||
            string.IsNullOrEmpty(managerOptions.Value.Email) ||
            string.IsNullOrEmpty(managerOptions.Value.Password))
        {
            logger.LogCritical("Information about manager must be set in configuration");
            return;
        }

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
