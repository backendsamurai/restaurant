using FluentValidation;
using Restaurant.API.Models.Product;


namespace Restaurant.API.Validators;

public sealed class UpdateProductModelValidator : AbstractValidator<UpdateProductModel>
{
    public UpdateProductModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("name cannot be empty")
            .MinimumLength(2).WithMessage("length of name must be greater than 2 characters")
            .WithName("name");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("description of product cannot be empty")
            .MinimumLength(10).WithMessage("description must contain 10 or more characters")
            .WithName("description");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("price must be greater than 0")
            .WithName("price");
    }
}
