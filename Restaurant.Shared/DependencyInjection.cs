using Microsoft.Extensions.DependencyInjection;
using Restaurant.Shared.Configurations;
using Restaurant.Shared.Database;

namespace Restaurant.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddShared(this IServiceCollection services) =>
        services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<ITransactional, Transactional>()
            .ConfigureOptions<JwtOptionsSetup>()
            .ConfigureOptions<ManagerOptionsSetup>();
}