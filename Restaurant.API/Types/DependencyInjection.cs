namespace Restaurant.API.Types;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomTypes(this IServiceCollection services)
    {
        return services.AddScoped<ITransactional, Transactional>();
    }
}