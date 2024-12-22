using Restaurant.Domain.Core.Guards;
using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.PaymentMethods.DomainEvents;

namespace Restaurant.Domain.PaymentMethods
{
    public sealed class PaymentMethod : AggregateRoot
    {
        public bool IsPrimary { get; private set; } = false;
        public PaymentCard PaymentCard { get; private set; }

        private PaymentMethod(PaymentCard paymentCard, bool isPrimary = false) : base(Guid.NewGuid())
        {
            Ensure.NotNull(paymentCard, "The payment card cannot be null", nameof(paymentCard));
            IsPrimary = isPrimary;
            PaymentCard = paymentCard;
        }

        public static PaymentMethod Create(PaymentCard paymentCard, bool isPrimary = false)
        {
            var paymentMethod = new PaymentMethod(paymentCard, isPrimary);
            paymentMethod.AddDomainEvent(new PaymentMethodCreatedDomainEvent(paymentMethod));

            return paymentMethod;
        }

        public void SetAsPrimary() => IsPrimary = true;
    }
}