using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order.GetOrders;

public sealed class GetOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetOrdersQuery, Result<List<OrderResponse>>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken) =>
        await orderService.GetOrdersAsync(request.Query);
}