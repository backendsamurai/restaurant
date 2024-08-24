namespace Restaurant.API.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IDeskService, DeskService>();
    }
}
