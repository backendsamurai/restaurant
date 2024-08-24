namespace Restaurant.API.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IDeskRepository, DeskRepository>();
    }
}