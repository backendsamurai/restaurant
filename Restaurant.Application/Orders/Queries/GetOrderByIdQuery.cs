using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.Orders.Queries;

public sealed record GetOrderByIdQuery(Guid OrderId);

public sealed class GetOrderByIdQueryHandler
{
    private readonly IOrdersService _ordersService;

    public GetOrderByIdQueryHandler(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    public async Task<Result<Order>> HandleAsync(GetOrderByIdQuery query, CancellationToken cancellationToken) =>
        await _ordersService.GetOrderAsync(query.OrderId, cancellationToken);
}
