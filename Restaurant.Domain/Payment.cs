
namespace Restaurant.Domain
{
    public sealed class Payment : Entity, IAuditableEntity
    {
        public Order Order { get; private set; } = default!;
        public decimal Bill { get; private set; }
        public decimal? Tip { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Payment() : base(Guid.NewGuid()) { }

        public Payment(Order order, decimal bill, decimal? tip) : base(Guid.NewGuid())
        {
            Order = order;
            Bill = bill;
            Tip = tip;
        }
    }
}