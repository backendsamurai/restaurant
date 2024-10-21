using Restaurant.API.Entities.Abstractions;

namespace Restaurant.API.Entities;

public sealed class Order : AuditableEntity
{
    public required Customer Customer { get; set; }
    public required Employee Waiter { get; set; }
    public required Desk Desk { get; set; }
    public required List<OrderLineItem> Items { get; set; }
    public OrderStatus Status { get; set; }
    public Payment? Payment { get; set; }
}
