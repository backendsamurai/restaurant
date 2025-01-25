
namespace Restaurant.Domain
{
    public sealed class Payment : Entity, IAuditableEntity
    {
        public Order Order { get; private set; }
        public decimal Bill { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Payment(Order order, decimal bill) : base(Guid.NewGuid())
        {
            Order = order;
            Bill = bill;
        }
    }
}