using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Contracts;
using Restaurant.Services.Implementations;

namespace Restaurant.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services) =>
        services
            .AddScoped<IConsumersService, ConsumersService>()
            .AddSingleton<IPasswordHasherService, PasswordHasherService>()
            .AddScoped<IMenuCategoriesService, MenuCategoriesService>()
            .AddScoped<IMenuItemsService, MenuItemsService>()
            .AddScoped<IOrdersService, OrdersService>();
}
