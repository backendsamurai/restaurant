using Microsoft.EntityFrameworkCore;
using Restaurant.Domain;
using Restaurant.Persistence.EntityConfigurations;

namespace Restaurant.Persistence;

public sealed class RestaurantDbContext : DbContext
{
    public DbSet<Consumer> Consumers { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }

    public DbSet<MenuCategory> MenuCategories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderLineItem> OrderLineItems { get; set; }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(EntityConfigurationsAssembly.GetAssembly());
    }
}
