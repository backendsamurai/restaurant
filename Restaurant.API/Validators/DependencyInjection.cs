using FluentValidation;
using Restaurant.API.Dto.Requests;

namespace Restaurant.API.Validators;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateCustomerRequest>, CreateCustomerRequestValidator>()
            .AddScoped<IValidator<UpdateCustomerRequest>, UpdateCustomerRequestValidator>()
            .AddScoped<IValidator<CreateDeskRequest>, CreateDeskRequestValidator>()
            .AddScoped<IValidator<UpdateDeskRequest>, UpdateDeskRequestValidator>()
            .AddScoped<IValidator<LoginUserRequest>, LoginUserRequestValidator>();
    }
}
