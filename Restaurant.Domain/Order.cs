namespace Restaurant.Domain
{
    public sealed class Order(Customer customer) : Entity(Guid.NewGuid()), IAuditableEntity
    {
        private readonly List<OrderLineItem> _items = [];
        private decimal _total = 0;

        public Customer Customer { get; private set; } = customer;
        public OrderStatus Status { get; private set; } = OrderStatus.Created;
        public IReadOnlyCollection<OrderLineItem> Items => _items.AsReadOnly();
        public decimal Total => Math.Floor(_total);
        public Payment? Payment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void AddItemToOrder(OrderLineItem item)
        {
            _items.Add(item);
            _total += item.Price;
        }

        public void AddPayment(Payment payment)
        {
            Payment = payment;
            Status = OrderStatus.Pending;
        }
    }
}