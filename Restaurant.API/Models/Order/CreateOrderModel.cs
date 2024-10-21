namespace Restaurant.API.Models.Order;

public sealed record CreateOrderModel(
    Guid CustomerId,
    Guid WaiterId,
    Guid DeskId,
    Dictionary<Guid, int> OrderItems
);