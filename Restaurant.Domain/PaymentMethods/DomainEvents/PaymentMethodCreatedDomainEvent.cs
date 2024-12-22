using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.PaymentMethods.DomainEvents
{
    public sealed class PaymentMethodCreatedDomainEvent : IDomainEvent
    {
        internal PaymentMethodCreatedDomainEvent(PaymentMethod paymentMethod) => PaymentMethod = paymentMethod;

        public PaymentMethod PaymentMethod { get; }
    }
}