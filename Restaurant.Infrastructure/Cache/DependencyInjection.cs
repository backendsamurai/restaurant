using Microsoft.Extensions.DependencyInjection;
using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.Infrastructure.Cache.Models;

namespace Restaurant.Infrastructure.Cache;

public static class DependencyInjection
{
    private static IServiceCollection AddRedisCaching(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "connectionString is null");

        return services
            .AddSingleton<IRedisConnectionProvider>((_) => new RedisConnectionProvider(connectionString));
    }

    private static IServiceCollection AddRedisIndexes(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IRedisConnectionProvider>()
            ?? throw new InvalidOperationException("cannot register indexes for redis cache");

        try
        {
            // Reset Cache (problem with sync new data after redis shutdown)
            provider.Connection.Execute("FLUSHALL");

            provider.Connection.CreateIndex(typeof(CustomerCache));
            provider.Connection.CreateIndex(typeof(ProductCategoryCache));
            provider.Connection.CreateIndex(typeof(ProductCache));
        }
        catch
        {
            // ignored
        }

        return services;
    }

    private static IServiceCollection AddRedisModels(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IRedisConnectionProvider>()
                   ?? throw new InvalidOperationException("cannot register models for redis cache");

        return services
            .AddScoped((_) => provider.RedisCollection<CustomerCache>())
            .AddScoped((_) => provider.RedisCollection<ProductCategoryCache>())
            .AddScoped((_) => provider.RedisCollection<ProductCache>());
    }

    public static IServiceCollection AddCache(this IServiceCollection services, string? redisConnectionString) =>
        services.AddRedisCaching(redisConnectionString).AddRedisIndexes().AddRedisModels();
}