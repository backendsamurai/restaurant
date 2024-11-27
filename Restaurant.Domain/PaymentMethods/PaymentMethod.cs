using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.PaymentMethods
{
    public sealed class PaymentMethod : Entity
    {
        public PaymentCard PaymentCard { get; private set; }
    }
}