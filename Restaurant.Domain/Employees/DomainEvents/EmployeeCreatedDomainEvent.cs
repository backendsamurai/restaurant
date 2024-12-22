using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.Employees.DomainEvents
{
    public sealed class EmployeeCreatedDomainEvent : IDomainEvent
    {
        internal EmployeeCreatedDomainEvent(Employee employee) => Employee = employee;
        public Employee Employee { get; }
    }
}