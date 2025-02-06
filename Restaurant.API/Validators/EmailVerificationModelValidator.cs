using FluentValidation;
using Restaurant.API.Models;
using Restaurant.API.Services.Implementations;

namespace Restaurant.API.Validators;

public sealed class EmailVerificationModelValidator : AbstractValidator<EmailVerificationModel>
{
    public EmailVerificationModelValidator()
    {
        RuleFor(e => e.OtpCode)
            .NotEmpty()
                .WithMessage("otp code cannot be empty")
            .Matches(@"^-?[0-9][0-9,\.]+$")
                .WithMessage("invalid otp code format")
            .Length(EmailVerificationService.OtpCodeLength)
                .WithMessage($"otp code must contain {EmailVerificationService.OtpCodeLength} digits")
            .WithName("otp_code");
    }
}
