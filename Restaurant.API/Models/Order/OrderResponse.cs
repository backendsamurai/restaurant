using Restaurant.API.Models.OrderLineItem;

namespace Restaurant.API.Models.Order;

public sealed record OrderResponse(
    Guid OrderId,
    Guid CustomerId,
    Guid WaiterId,
    Guid DeskId,
    string Status,
    List<OrderLineItemResponse> Items,
    Entities.Payment Payment,
    DateTime CreatedAt,
    DateTime UpdatedAt
);