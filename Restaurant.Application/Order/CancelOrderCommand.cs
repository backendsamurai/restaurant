using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Order;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest<Result>;

public sealed class CancelOrderCommandHandler(IOrderService orderService) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken) =>
        await orderService.CancelOrderAsync(request.OrderId);
}