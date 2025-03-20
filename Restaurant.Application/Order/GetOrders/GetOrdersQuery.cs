using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order.GetOrders;

public sealed class GetOrdersQuery : IRequest<Result<List<OrderResponse>>>
{
    public required OrderQuery Query { get; set; }
}