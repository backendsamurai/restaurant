using Restaurant.API.Entities;
using Restaurant.API.Models.Order;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Contracts;

public interface IOrderService
{
    public Task<Result<List<OrderResponse>>> GetOrdersAsync(OrderQuery orderQuery);
    public Task<Result<List<OrderResponse>>> GetOrdersByCustomerAsync(Guid customerId);
    public Task<Result<List<OrderResponse>>> GetOrdersByEmployeeAsync(Guid employeeId);
    public Task<Result<List<OrderResponse>>> GetOrdersByDeskAsync(Guid deskId);
    public Task<Result<OrderResponse>> GetOrderByIdAsync(Guid orderId);
    public Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderModel createOrderModel);
    public Task<Result<OrderResponse>> AddPaymentAsync(Guid orderId, Guid paymentId);
    public Task<Result> CloseOrderAsync(Guid orderId);
}