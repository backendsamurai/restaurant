using FluentValidation;
using Restaurant.API.Validators.Helpers;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.API.Validators;

public sealed class CreateCustomerModelValidator : AbstractValidator<CreateCustomerModel>
{
    public CreateCustomerModelValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
                .WithMessage("name field is required")
            .NotEmpty()
                .WithMessage("name cannot be empty")
            .MinimumLength(2)
                .WithMessage("name must be longer than 2 characters")
            .Must((v) => v is not null && v.Length > 0 && char.IsUpper(v[0]))
                .WithMessage("the name must be capitalized")
            .WithName("name");

        RuleFor(c => c.Email)
            .NotNull()
                .WithMessage("email field is required")
            .NotEmpty()
                .WithMessage("email cannot be empty")
            .Must(EmailValidatorHelper.IsEmailValid)
                .WithMessage("the email address format is incorrect")
            .WithName("email");

        RuleFor(c => c.Password)
            .NotNull()
                .WithMessage("password field is required")
            .NotEmpty()
                .WithMessage("password cannot be empty")
            .MinimumLength(8)
                .WithMessage("password must be longer than 8 characters")
            .MaximumLength(16)
                .WithMessage("the length of the password should not exceed 16 characters")
            .WithName("password");
    }
}