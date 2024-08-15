using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data.EntityConfigurations;
using Restaurant.API.Entities;

namespace Restaurant.API.Data;

public sealed class RestaurantDbContext(DbContextOptions options) : DbContext(options)
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
    }
}
