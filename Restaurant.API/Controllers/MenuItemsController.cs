using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.MenuItems.Commands;
using Restaurant.Application.MenuItems.Queries;
using Restaurant.Domain;
using Restaurant.Services.DTOs.MenuItem;
using Wolverine;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("v1/menu/items")]
[EndpointGroupName("menuItems")]
public sealed class MenuItemsController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public MenuItemsController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [TranslateResultToActionResult]
    [HttpGet]
    public async Task<Result<List<MenuItem>>> GetMenuItemsAsync([FromQuery(Name = "name")] string? name) =>
        await _messageBus.InvokeAsync<Result<List<MenuItem>>>(new GetMenuItemsQuery(name));

    [TranslateResultToActionResult]
    [HttpGet("{menuItemId:guid}")]
    public async Task<Result<MenuItem>> GetMenuItemAsync([FromRoute] Guid menuItemId) =>
        await _messageBus.InvokeAsync<Result<MenuItem>>(new GetMenuItemByIdQuery(menuItemId));

    [TranslateResultToActionResult]
    [HttpPost]
    public async Task<Result<MenuItem>> CreateMenuItemAsync([FromBody] CreateMenuItemDTO dto) =>
        await _messageBus.InvokeAsync<Result<MenuItem>>(new CreateMenuItemCommand(dto));

    [TranslateResultToActionResult]
    [HttpPatch("{menuItemId:guid}")]
    public async Task<Result<MenuItem>> UpdateMenuItemAsync([FromRoute] Guid menuItemId, [FromBody] UpdateMenuItemDTO dto) =>
        await _messageBus.InvokeAsync<Result<MenuItem>>(new UpdateMenuItemCommand(menuItemId, dto));

    [TranslateResultToActionResult]
    [HttpDelete("{menuItemId:guid}")]
    public async Task<Result> RemoveMenuItemAsync([FromRoute] Guid menuItemId) =>
        await _messageBus.InvokeAsync<Result>(new RemoveMenuItemCommand(menuItemId));
}
