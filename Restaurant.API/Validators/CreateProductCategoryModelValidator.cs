using FluentValidation;
using Restaurant.API.Models.ProductCategory;

namespace Restaurant.API.Validators;

public class CreateProductCategoryModelValidator : AbstractValidator<CreateProductCategoryModel>
{
    public CreateProductCategoryModelValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("field 'name' cannot be empty")
            .MinimumLength(3).WithMessage("field 'name' must contain 3 or more characters")
            .WithName("name");
    }
}
