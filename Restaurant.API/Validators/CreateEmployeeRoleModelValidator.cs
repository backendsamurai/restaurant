using FluentValidation;
using Restaurant.API.Models.EmployeeRole;

namespace Restaurant.API.Validators
{
    public class CreateEmployeeRoleModelValidator : AbstractValidator<CreateEmployeeRoleModel>
    {
        public CreateEmployeeRoleModelValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("name of role must be set")
                .NotEmpty().WithMessage("name of role cannot be empty")
                .WithName("name");
        }
    }
}