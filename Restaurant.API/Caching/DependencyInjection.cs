using Redis.OM;
using Restaurant.API.Caching.Models;

namespace Restaurant.API.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "connectionString is null");

        var provider = new RedisConnectionProvider(connectionString);

        return services.AddScoped((_) => provider);
    }

    public static IServiceCollection AddRedisIndexes(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<RedisConnectionProvider>()
            ?? throw new InvalidOperationException("cannot register indexes for redis cache");

        provider.Connection.CreateIndex(typeof(DeskCacheModel));
        provider.Connection.CreateIndex(typeof(CustomerCacheModel));

        return services;
    }


    public static IServiceCollection AddRedisModels(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<RedisConnectionProvider>()
                   ?? throw new InvalidOperationException("cannot register indexes for redis cache");

        return services
            .AddScoped((_) => provider.RedisCollection<DeskCacheModel>())
            .AddScoped((_) => provider.RedisCollection<CustomerCacheModel>());
    }
}