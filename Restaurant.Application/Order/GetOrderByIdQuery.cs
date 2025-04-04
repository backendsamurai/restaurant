using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<Result<OrderResponse>>;

public sealed class GetOrderByIdQueryHandler(IOrderService orderService) : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken) =>
        await orderService.GetOrderByIdAsync(request.OrderId);
}