using Microsoft.EntityFrameworkCore;

namespace Restaurant.API.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string? connectionString)
    {
        return services.AddDbContext<RestaurantDbContext>();
    }
}
