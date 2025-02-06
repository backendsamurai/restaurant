using Restaurant.API.Services.Contracts;
using Restaurant.API.Services.Implementations;

namespace Restaurant.API.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services) =>
        services
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IEmailVerificationService, EmailVerificationService>()
            .AddScoped<IProductCategoryService, ProductCategoryService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IOrderService, OrderService>();
}
