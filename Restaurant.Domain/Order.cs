namespace Restaurant.Domain;

public sealed class Order : Entity<Guid>, IAuditable
{
    private readonly List<OrderLineItem> _items = [];

    private decimal _total = 0;

    public Consumer? Consumer { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderLineItem> Items => _items.AsReadOnly();

    public decimal Total => Math.Floor(_total);

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    private Order() { }

    public Order(Guid id, Consumer? consumer) : base(id)
    {
        Consumer = consumer;
        Status = OrderStatus.Initiated;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void AddItemToOrder(OrderLineItem item)
    {
        _items.Add(item);
        _total += item.Price;
    }

    public void ChangeStatus(OrderStatus status) => Status = status;
}
