using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Customer.GetCustomerOrders;

public sealed class GetCustomerOrdersQueryHandler(IOrderService orderService) : IRequestHandler<GetCustomerOrdersQuery, Result<List<OrderResponse>>>
{
    public async Task<Result<List<OrderResponse>>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken) =>
        await orderService.GetOrdersByCustomerAsync(request.CustomerId);
}