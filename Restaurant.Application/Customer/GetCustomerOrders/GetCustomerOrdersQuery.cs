using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Application.Customer.GetCustomerOrders;

public sealed class GetCustomerOrdersQuery : IRequest<Result<List<OrderResponse>>>
{
    public Guid CustomerId { get; set; }
}