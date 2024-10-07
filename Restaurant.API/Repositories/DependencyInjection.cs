namespace Restaurant.API.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}