using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Services.DTOs.Order;

namespace Restaurant.Application.Orders.Commands;

public sealed record CreateOrderCommand(CreateOrderDTO Dto);

public sealed class CreateOrderCommandHandler
{
    private readonly IOrdersService _ordersService;

    public CreateOrderCommandHandler(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    public async Task<Result<Order>> HandleAsync(CreateOrderCommand cmd, CancellationToken cancellationToken) =>
        await _ordersService.CreateOrderAsync(cmd.Dto, cancellationToken);
}
