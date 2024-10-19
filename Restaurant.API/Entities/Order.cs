namespace Restaurant.API.Entities;

public sealed class Order : Entity
{
    public required Customer Customer { get; set; }
    public required Employee Waiter { get; set; }
    public required Desk Desk { get; set; }
    public List<OrderLineItem>? Items { get; set; }
    public OrderStatus Status { get; set; }
    public Payment? Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
