using FluentValidation;
using Restaurant.API.Models.Order;

namespace Restaurant.API.Validators;

public sealed class CreateOrderModelValidator : AbstractValidator<CreateOrderModel>
{
    public CreateOrderModelValidator()
    {
        RuleFor(e => e.OrderItems)
            .NotEmpty()
            .WithMessage("order items cannot be empty");
    }
}