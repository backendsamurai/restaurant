namespace Restaurant.API.Entities;

public sealed class Order : Entity
{
    public Customer? Customer { get; set; }
    public required Employee Waiter { get; set; }
    public required Desk Desk { get; set; }
    public required List<OrderLineItem> Items { get; set; }
    public OrderStatus Status { get; set; }
    public Payment? Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
