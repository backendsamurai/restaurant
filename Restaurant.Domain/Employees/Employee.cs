using Restaurant.Domain.BonusCards;
using Restaurant.Domain.Core.Abstractions;
using Restaurant.Domain.Core.Guards;
using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Employees.DomainEvents;
using Restaurant.Domain.Users;

namespace Restaurant.Domain.Employees
{
    public sealed class Employee : AggregateRoot, IAuditableEntity
    {
        private string _passwordHash;
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }
        public Address Address { get; private set; } = Address.Empty;
        public BonusCard BonusCard { get; private set; }
        public EmployeeRole Role { get; private set; }
        public WorkStatus Status { get; private set; }
        public WorkPeriod WorkPeriod { get; private set; }
        public DateTime CreatedOnUTC { get; }
        public DateTime? ModifiedOnUTC { get; }

        private Employee(
            FirstName firstName,
            LastName lastName,
            Email email,
            PhoneNumber phoneNumber,
            string passwordHash,
            DateOfBirth dateOfBirth,
            BonusCard bonusCard,
            EmployeeRole role,
            WorkPeriod workPeriod,
            WorkStatus status
        ) : base(Guid.NewGuid())
        {
            Ensure.NotNull(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotNull(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotNull(email, "The email is required.", nameof(email));
            Ensure.NotNull(phoneNumber, "The phone number is required.", nameof(phoneNumber));
            Ensure.NotNull(bonusCard, "The bonus card is required.", nameof(bonusCard));
            Ensure.NotNull(passwordHash, "The password hash is required.", nameof(passwordHash));
            Ensure.NotNull(role, "The employee role is required.", nameof(role));
            Ensure.NotNull(workPeriod, "The work period is required.", nameof(workPeriod));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            BonusCard = bonusCard;
            Role = role;
            Status = status;
            WorkPeriod = workPeriod;
            _passwordHash = passwordHash;
        }

        public static Employee Create(
            FirstName firstName,
            LastName lastName,
            Email email,
            PhoneNumber phoneNumber,
            string passwordHash,
            DateOfBirth dateOfBirth,
            BonusCard bonusCard,
            EmployeeRole role,
            WorkPeriod workPeriod,
            WorkStatus workStatus
        )
        {
            var employee = new Employee(firstName, lastName, email, phoneNumber, passwordHash, dateOfBirth, bonusCard, role, workPeriod, workStatus);

            employee.AddDomainEvent(new EmployeeCreatedDomainEvent(employee));

            return employee;
        }

        public bool ChangePassword(string newPasswordHash)
        {
            Ensure.NotEmpty(newPasswordHash, "The password hash is required.", nameof(newPasswordHash));

            if (newPasswordHash == _passwordHash)
                return false;

            _passwordHash = newPasswordHash;
            return true;
        }

        public void AddAddress(Address address)
        {
            if (address == Address.Empty)
                throw new ArgumentException("The address is required.", nameof(address));

            if (Address != Address.Empty)
                throw new InvalidOperationException("Cannot set address value because it already has a value.");

            Address = address;
        }

        public void UpdateAddress(Address address)
        {
            if (address == Address.Empty)
                throw new ArgumentException("The address is required.", nameof(address));

            Address = address;
        }

        public void RemoveAddress() => Address = Address.Empty;
    }
}