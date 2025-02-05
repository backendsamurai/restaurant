using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.API.Caching.Models;

namespace Restaurant.API.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString), "connectionString is null");

        return services
            .AddSingleton<IRedisConnectionProvider>((_) => new RedisConnectionProvider(connectionString));
    }

    public static IServiceCollection AddRedisIndexes(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IRedisConnectionProvider>()
            ?? throw new InvalidOperationException("cannot register indexes for redis cache");

        try
        {
            // Reset Cache (problem with sync new data after redis shutdown)
            provider.Connection.Execute("FLUSHALL");

            provider.Connection.CreateIndex(typeof(EmployeeRoleCacheModel));
            provider.Connection.CreateIndex(typeof(EmployeeCacheModel));
            provider.Connection.CreateIndex(typeof(CustomerCacheModel));
            provider.Connection.CreateIndex(typeof(DeskCacheModel));
            provider.Connection.CreateIndex(typeof(ProductCategoryModel));
            provider.Connection.CreateIndex(typeof(ProductModel));
        }
        catch
        {
            // ignored
        }

        return services;
    }

    public static IServiceCollection AddRedisModels(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var provider = serviceProvider.GetRequiredService<IRedisConnectionProvider>()
                   ?? throw new InvalidOperationException("cannot register models for redis cache");

        return services
            .AddScoped((_) => provider.RedisCollection<DeskCacheModel>())
            .AddScoped((_) => provider.RedisCollection<CustomerCacheModel>())
            .AddScoped((_) => provider.RedisCollection<EmployeeCacheModel>())
            .AddScoped((_) => provider.RedisCollection<EmployeeRoleCacheModel>())
            .AddScoped((_) => provider.RedisCollection<ProductCategoryModel>())
            .AddScoped((_) => provider.RedisCollection<ProductModel>());
    }
}