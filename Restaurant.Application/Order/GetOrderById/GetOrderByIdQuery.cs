using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order.GetOrderById;

public sealed class GetOrderByIdQuery : IRequest<Result<OrderResponse>>
{
    public Guid OrderId { get; set; }
}