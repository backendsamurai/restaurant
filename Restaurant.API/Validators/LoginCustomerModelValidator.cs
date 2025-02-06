using FluentValidation;
using Restaurant.API.Models.Customer;
using Restaurant.API.Validators.Helpers;

namespace Restaurant.API.Validators;

public sealed class LoginCustomerModelValidator : AbstractValidator<LoginCustomerModel>
{
    public LoginCustomerModelValidator()
    {
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
