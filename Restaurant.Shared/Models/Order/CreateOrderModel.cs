namespace Restaurant.Shared.Models.Order;

public sealed record CreateOrderModel(
    Guid CustomerId,
    Dictionary<Guid, int> OrderItems
);