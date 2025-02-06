using Restaurant.API.Models.OrderLineItem;

namespace Restaurant.API.Models.Order;

public sealed record OrderResponse(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    List<OrderLineItemResponse> Items,
    Domain.Payment Payment,
    DateTime CreatedAt,
    DateTime UpdatedAt
);