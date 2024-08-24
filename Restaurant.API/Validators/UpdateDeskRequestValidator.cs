using FluentValidation;
using Restaurant.API.Dto.Requests;

namespace Restaurant.API.Validators;

public sealed class UpdateDeskRequestValidator : AbstractValidator<UpdateDeskRequest>
{
    public UpdateDeskRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("name field is required")
            .NotEmpty()
                .WithMessage("name field must be not empty")
            .WithName("name");
    }
}
