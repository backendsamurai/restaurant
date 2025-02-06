
namespace Restaurant.Domain
{
    public sealed class Payment(Order order, decimal bill) : Entity(Guid.NewGuid()), IAuditableEntity
    {
        public Order Order { get; private set; } = order;
        public decimal Bill { get; private set; } = bill;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}