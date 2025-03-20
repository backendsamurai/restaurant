using Microsoft.Extensions.DependencyInjection;
using Restaurant.Services.Contracts;
using Restaurant.Services.Implementations;

namespace Restaurant.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services) =>
        services
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IProductCategoryService, ProductCategoryService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IOrderService, OrderService>();
}
