using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Entities;
using Restaurant.API.Models.Order;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

public record AddPaymentModel(Guid PaymentId);

[ApiController]
[Route("orders")]
public sealed class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [ApplyResult]
    [HttpGet]
    public async Task<Result<List<OrderResponse>>> GetOrders(
        [FromQuery(Name = "status")] OrderStatus? status,
        [FromQuery(Name = "customerId")] Guid? customerId,
        [FromQuery(Name = "employeeId")] Guid? employeeId,
        [FromQuery(Name = "deskId")] Guid? deskId)
    {
        if (status is not null)
            return await _orderService.GetOrdersAsync(status.GetValueOrDefault());

        if (customerId is not null)
            return await _orderService.GetOrdersByCustomerAsync(customerId.GetValueOrDefault());

        if (employeeId is not null)
            return await _orderService.GetOrdersByEmployeeAsync(employeeId.GetValueOrDefault());

        if (deskId is not null)
            return await _orderService.GetOrderByDeskAsync(deskId.GetValueOrDefault());

        return Result.Error(
            code: "ODC-000-001",
            type: "invalid query",
            message: "Incorrect query parameters",
            detail: "Provide correct query parameters"
        );
    }

    [ApplyResult]
    [HttpGet("{orderId:guid}")]
    public async Task<Result<OrderResponse>> GetOrderById([FromRoute(Name = "orderId")] Guid orderId) =>
        await _orderService.GetOrderByIdAsync(orderId);

    [ApplyResult]
    [HttpPost]
    public async Task<Result<OrderResponse>> CreateOrder([FromBody] CreateOrderModel createOrderModel) =>
        await _orderService.CreateOrderAsync(createOrderModel);

    [ApplyResult]
    [HttpPatch("{orderId:guid}")]
    public async Task<Result<OrderResponse>> AddPaymentToOrder([FromRoute(Name = "orderId")] Guid orderId, [FromBody] AddPaymentModel addPaymentModel) =>
        await _orderService.AddPaymentAsync(orderId, addPaymentModel.PaymentId);

    [ApplyResult]
    [HttpDelete("{orderId:guid}/close")]
    public async Task<Result> CloseOrder([FromRoute(Name = "orderId")] Guid orderId) =>
        await _orderService.CloseOrderAsync(orderId);
}