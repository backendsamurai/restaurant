using FluentValidation;
using Restaurant.Shared.Models.Customer;
using Restaurant.Shared.Models.Order;
using Restaurant.Shared.Models.Payment;
using Restaurant.Shared.Models.Product;
using Restaurant.Shared.Models.ProductCategory;

namespace Restaurant.API.Validators;

public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateCustomerModel>, CreateCustomerModelValidator>()
            .AddScoped<IValidator<UpdateCustomerModel>, UpdateCustomerModelValidator>()
            .AddScoped<IValidator<LoginCustomerModel>, LoginCustomerModelValidator>()
            .AddScoped<IValidator<CreateProductCategoryModel>, CreateProductCategoryModelValidator>()
            .AddScoped<IValidator<UpdateProductCategoryModel>, UpdateProductCategoryModelValidator>()
            .AddScoped<IValidator<CreateProductModel>, CreateProductModelValidator>()
            .AddScoped<IValidator<UpdateProductModel>, UpdateProductModelValidator>()
            .AddScoped<IValidator<CreatePaymentModel>, CreatePaymentModelValidator>()
            .AddScoped<IValidator<CreateOrderModel>, CreateOrderModelValidator>();
    }
}
