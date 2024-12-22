using Restaurant.Domain.Core.Guards;
using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Employees
{
    public sealed class EmployeeRole : Entity
    {
        private const int MinLength = 3;
        public string Name { get; private set; }

        private EmployeeRole(string name)
        {
            Ensure.NotEmpty(name, "The role name cannot be empty", nameof(name));
            Name = name;
        }

        public static Result<EmployeeRole> Create(string roleName)
            => Result.Create(roleName, EmployeeErrors.EmployeeRole.NullOrEmpty)
                .Ensure(r => !string.IsNullOrWhiteSpace(r), EmployeeErrors.EmployeeRole.NullOrEmpty)
                .Ensure(r => r.Length >= MinLength, EmployeeErrors.EmployeeRole.TooShort)
                .Map(r => new EmployeeRole(r));

        public Result ChangeName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                Name = newName;
                return Result.Success();
            }

            return Result.Failure(EmployeeErrors.EmployeeRole.NullOrEmpty);
        }
    }
}