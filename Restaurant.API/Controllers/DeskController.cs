using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;
using Restaurant.API.Models.Order;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("desks")]
public sealed class DeskController(IDeskService deskService, IOrderService orderService) : ControllerBase
{
    private readonly IDeskService _deskService = deskService;
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    public async Task<Result<List<Desk>>> GetAllDesks() =>
        await _deskService.GetAllDesksAsync();

    [HttpGet("{id:guid}")]
    public async Task<Result<Desk>> GetDeskById([FromRoute(Name = "id")] Guid id) =>
        await _deskService.GetDeskByIdAsync(id);

    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPost]
    public async Task<Result<Desk>> CreateDesk([FromBody] CreateDeskModel createDeskModel) =>
        await _deskService.CreateDeskAsync(createDeskModel);

    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<Desk>> UpdateDesk(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateDeskModel updateDeskModel
    ) => await _deskService.UpdateDeskAsync(id, updateDeskModel);

    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveDesk([FromRoute(Name = "id")] Guid id) =>
        await _deskService.RemoveDeskAsync(id);

    [HttpGet("{deskId:guid}/orders")]
    public async Task<Result<List<OrderResponse>>> GetOrders([FromRoute(Name = "deskId")] Guid deskId) =>
        await _orderService.GetOrdersByDeskAsync(deskId);
}
