using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.Services.Contracts;

public interface IOrderService
{
    public Task<Result<List<OrderResponse>>> GetOrdersAsync(OrderQuery orderQuery);
    public Task<Result<List<OrderResponse>>> GetOrdersByCustomerAsync(Guid customerId);
    public Task<Result<OrderResponse>> GetOrderByIdAsync(Guid orderId);
    public Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderModel createOrderModel);
    public Task<Result> CancelOrderAsync(Guid orderId);
}