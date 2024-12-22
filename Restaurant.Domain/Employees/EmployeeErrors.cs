using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Employees
{
    public static class EmployeeErrors
    {
        public static class EmployeeRole
        {
            public static readonly Error NullOrEmpty = new("EmployeeRole.NullOrEmpty", "Employee role name cannot be empty");
            public static readonly Error TooShort = new("EmployeeRole.TooShort", "The employee role name must contain 3 or more characters");
        }
    }
}