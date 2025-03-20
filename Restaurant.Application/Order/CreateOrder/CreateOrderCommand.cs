using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order.CreateOrder;

public sealed class CreateOrderCommand : IRequest<Result<OrderResponse>>
{
    public Guid CustomerId { get; set; }
    public Dictionary<Guid, int> OrderItems { get; set; } = [];
}