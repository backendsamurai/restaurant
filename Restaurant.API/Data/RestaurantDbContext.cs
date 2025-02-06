using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Restaurant.API.Data.EntityConfigurations;
using Restaurant.Domain;

namespace Restaurant.API.Data;

public sealed class RestaurantDbContext(
    IConfiguration configuration,
    ILogger<RestaurantDbContext> logger
) : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
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
    }
}
