using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Models.Order;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrderController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [ServiceFilter<ApplyResultAttribute>]
    [HttpGet]
    public async Task<Result<List<OrderResponse>>> GetOrders()
    {
        var orderQuery = TransformQueryIntoObject<OrderQuery>.Transform(HttpContext.Request.Query);

        return await _orderService.GetOrdersAsync(orderQuery);
    }

    [ServiceFilter<ApplyResultAttribute>]
    [HttpGet("{orderId:guid}")]
    public async Task<Result<OrderResponse>> GetOrderById([FromRoute(Name = "orderId")] Guid orderId) =>
        await _orderService.GetOrderByIdAsync(orderId);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpPost]
    public async Task<Result<OrderResponse>> CreateOrder([FromBody] CreateOrderModel createOrderModel) =>
        await _orderService.CreateOrderAsync(createOrderModel);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpPatch("{orderId:guid}")]
    public async Task<Result<OrderResponse>> AddPaymentToOrder([FromRoute(Name = "orderId")] Guid orderId, [FromBody] AddPaymentModel addPaymentModel) =>
        await _orderService.AddPaymentAsync(orderId, addPaymentModel.PaymentId);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpDelete("{orderId:guid}/close")]
    public async Task<Result> CloseOrder([FromRoute(Name = "orderId")] Guid orderId) =>
        await _orderService.CloseOrderAsync(orderId);
}