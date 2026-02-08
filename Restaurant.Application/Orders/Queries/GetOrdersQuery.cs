using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.Contracts;

namespace Restaurant.Application.Orders.Queries;

public sealed record GetOrdersQuery(Guid? ConsumerId);

public sealed class GetOrdersQueryHandler
{
    private readonly IOrdersService _ordersService;

    public GetOrdersQueryHandler(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    public async Task<Result<List<Order>>> HandleAsync(GetOrdersQuery query, CancellationToken cancellationToken) =>
        await _ordersService.GetOrdersAsync(query.ConsumerId, cancellationToken);
}
