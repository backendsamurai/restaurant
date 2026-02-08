using Ardalis.Result;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.Orders.Commands;

public sealed record CancelOrderCommand(Guid OrderId);

public sealed class CancelOrderCommandHandler
{
    private readonly IOrdersService _ordersService;

    public CancelOrderCommandHandler(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    public async Task<Result> HandleAsync(CancelOrderCommand cmd, CancellationToken cancellationToken) =>
        await _ordersService.CancelOrderAsync(cmd.OrderId, cancellationToken);
}
