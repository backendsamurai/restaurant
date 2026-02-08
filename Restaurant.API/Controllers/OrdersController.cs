using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Orders.Commands;
using Restaurant.Application.Orders.Queries;
using Restaurant.Domain;
using Restaurant.Services.DTOs.Order;
using Wolverine;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("v1/orders")]
[EndpointGroupName("orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public OrdersController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [TranslateResultToActionResult]
    [HttpGet]
    public async Task<Result<List<Order>>> GetOrdersAsync([FromQuery(Name = "consumerId")] Guid? consumerId) =>
        await _messageBus.InvokeAsync<Result<List<Order>>>(new GetOrdersQuery(consumerId));

    [TranslateResultToActionResult]
    [HttpGet("{orderId:guid}")]
    public async Task<Result<Order>> GetOrderAsync([FromRoute] Guid orderId) =>
        await _messageBus.InvokeAsync<Result<Order>>(new GetOrderByIdQuery(orderId));

    [TranslateResultToActionResult]
    [HttpPost]
    public async Task<Result<Order>> CreateOrderAsync([FromBody] CreateOrderDTO dto) =>
        await _messageBus.InvokeAsync<Result<Order>>(new CreateOrderCommand(dto));

    [TranslateResultToActionResult]
    [HttpDelete("{orderId:guid}/cancel")]
    public async Task<Result> CancelOrderAsync([FromRoute] Guid orderId) =>
        await _messageBus.InvokeAsync<Result>(new CancelOrderCommand(orderId));
}
