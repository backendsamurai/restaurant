namespace Restaurant.API.Attributes;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomAttributes(this IServiceCollection services) =>
        services.AddScoped<ApplyResultAttribute>();
}