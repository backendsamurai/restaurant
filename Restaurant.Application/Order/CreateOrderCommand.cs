using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Order;

public sealed record CreateOrderCommand(Guid CustomerId, Dictionary<Guid, int> OrderItems) : IRequest<Result<OrderResponse>>;

public sealed class CreateOrderCommandHandler(IOrderService orderService) : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken) =>
        await orderService.CreateOrderAsync(new CreateOrderModel(request.CustomerId, request.OrderItems));
}