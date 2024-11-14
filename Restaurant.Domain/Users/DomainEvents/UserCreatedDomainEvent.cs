using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.Users.DomainEvents
{
    public sealed class UserCreatedDomainEvent : IDomainEvent
    {
        internal UserCreatedDomainEvent(User user) => User = user;

        public User User { get; }
    }
}