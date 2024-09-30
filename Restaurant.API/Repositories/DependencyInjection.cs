using Restaurant.API.Repositories.Contracts;
using Restaurant.API.Repositories.Implementations;

namespace Restaurant.API.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IDeskRepository, DeskRepository>()
            .AddScoped<IEmployeeRepository, EmployeeRepository>()
            .AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>()
            .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}