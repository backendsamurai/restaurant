using FluentValidation;
using Restaurant.Shared.Models.Payment;

namespace Restaurant.API.Validators;

public sealed class CreatePaymentModelValidator : AbstractValidator<CreatePaymentModel>
{
    public CreatePaymentModelValidator()
    {
        RuleFor(e => e.Bill).GreaterThan(0.0M).WithMessage("bill must be greater than 0");
    }
}