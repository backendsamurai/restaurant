using Restaurant.Shared.Models.OrderLineItem;

namespace Restaurant.Shared.Models.Order;

public sealed record OrderResponse(
    Guid OrderId,
    Guid CustomerId,
    string Status,
    List<OrderLineItemResponse> Items,
    Domain.Payment Payment,
    DateTime CreatedAt,
    DateTime UpdatedAt
);