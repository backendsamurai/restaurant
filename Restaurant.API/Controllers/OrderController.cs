using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Models.Order;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<OrderResponse>>> GetOrders()
    {
        var orderQuery = TransformQueryIntoObject<OrderQuery>.Transform(HttpContext.Request.Query);

        return await orderService.GetOrdersAsync(orderQuery);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<Result<OrderResponse>> GetOrderById([FromRoute(Name = "orderId")] Guid orderId) =>
        await orderService.GetOrderByIdAsync(orderId);

    [HttpPost]
    public async Task<Result<OrderResponse>> CreateOrder([FromBody] CreateOrderModel createOrderModel) =>
        await orderService.CreateOrderAsync(createOrderModel);

    [HttpDelete("{orderId:guid}/cancel")]
    public async Task<Result> CloseOrder([FromRoute(Name = "orderId")] Guid orderId) =>
        await orderService.CancelOrderAsync(orderId);
}