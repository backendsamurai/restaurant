using Ardalis.Result;
using Restaurant.Domain;
using Restaurant.Services.DTOs.Order;

namespace Restaurant.Services.Contracts;

public interface IOrdersService
{
    Task<Result<List<Order>>> GetOrdersAsync(Guid? consumerId = null, CancellationToken cancellationToken = default);

    Task<Result<Order>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<Result<Order>> CreateOrderAsync(CreateOrderDTO createOrderDTO, CancellationToken cancellationToken = default);

    Task<Result> CancelOrderAsync(Guid orderId, CancellationToken cancellationToken = default);
}
