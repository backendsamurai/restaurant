using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Order.CancelOrder;

public sealed class CancelOrderCommand : IRequest<Result>
{
    public Guid OrderId { get; set; }
}