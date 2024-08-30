using FluentValidation;
using Restaurant.API.Dto.Requests;

namespace Restaurant.API.Validators
{
    public class UpdateEmployeeRoleRequestValidator : AbstractValidator<UpdateEmployeeRoleRequest>
    {
        public UpdateEmployeeRoleRequestValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("name of role must be set")
                .NotEmpty().WithMessage("name of role cannot be empty")
                .WithName("name");
        }
    }
}