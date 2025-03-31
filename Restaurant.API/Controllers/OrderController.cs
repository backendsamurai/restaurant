using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.Application.Order;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Order;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<OrderResponse>>> GetOrders()
    {
        var orderQuery = TransformQueryIntoObject<OrderQuery>.Transform(HttpContext.Request.Query);

        return await mediator.Send(new GetOrdersQuery(orderQuery));
    }

    [HttpGet("{orderId:guid}")]
    public async Task<Result<OrderResponse>> GetOrderById([FromRoute(Name = "orderId")] Guid orderId) =>
        await mediator.Send(new GetOrderByIdQuery(orderId));

    [HttpPost]
    public async Task<Result<OrderResponse>> CreateOrder([FromBody] CreateOrderCommand command) => await mediator.Send(command);

    [HttpDelete("{orderId:guid}/cancel")]
    public async Task<Result> CancelOrder([FromRoute(Name = "orderId")] Guid orderId) =>
        await mediator.Send(new CancelOrderCommand(orderId));
}