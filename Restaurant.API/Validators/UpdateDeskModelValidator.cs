using FluentValidation;
using Restaurant.API.Models.Desk;

namespace Restaurant.API.Validators;

public sealed class UpdateDeskModelValidator : AbstractValidator<UpdateDeskModel>
{
    public UpdateDeskModelValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("name field is required")
            .NotEmpty()
                .WithMessage("name field must be not empty")
            .WithName("name");
    }
}
