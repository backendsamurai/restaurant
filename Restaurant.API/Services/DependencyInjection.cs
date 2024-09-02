using Restaurant.API.Services.Contracts;
using Restaurant.API.Services.Implementations;

namespace Restaurant.API.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInternalServices(this IServiceCollection services) =>
        services
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IDeskService, DeskService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IEmployeeRoleService, EmployeeRoleService>()
            .AddScoped<IEmployeeService, EmployeeService>();
}
