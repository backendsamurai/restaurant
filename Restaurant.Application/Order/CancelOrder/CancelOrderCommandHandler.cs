using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Order.CancelOrder;

public sealed class CancelOrderCommandHandler(IOrderService orderService) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken) =>
        await orderService.CancelOrderAsync(request.OrderId);
}