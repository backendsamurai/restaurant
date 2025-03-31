
namespace Restaurant.Domain
{
    public sealed class Payment : Entity, IAuditableEntity
    {
        public Order Order { get; private set; } = default!;
        public decimal Bill { get; private set; }
        public decimal? Tip { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private Payment() { }

        private Payment(Guid id, Order order, decimal bill, decimal? tip) : base(id)
        {
            Order = order;
            Bill = bill;
            Tip = tip;
        }

        public static Payment Create(Guid id, Order order, decimal bill, decimal? tip)
        {
            return new Payment(id, order, bill, tip);
        }
    }
}