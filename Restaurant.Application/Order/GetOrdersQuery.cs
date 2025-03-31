using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order;

public sealed record GetOrdersQuery(OrderQuery Query) : IRequest<Result<List<OrderResponse>>>;

public sealed class GetOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetOrdersQuery, Result<List<OrderResponse>>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken) =>
        await orderService.GetOrdersAsync(request.Query);
}