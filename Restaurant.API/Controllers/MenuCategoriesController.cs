using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.MenuCategories.Commands;
using Restaurant.Application.MenuCategories.Queries;
using Restaurant.Domain;
using Restaurant.Services.DTOs.MenuCategory;
using Wolverine;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("v1/menu/categories")]
[EndpointGroupName("menuCategories")]
public sealed class MenuCategoriesController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public MenuCategoriesController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [TranslateResultToActionResult]
    [HttpGet]
    public async Task<Result<List<MenuCategory>>> GetMenuCategoriesAsync([FromQuery(Name = "name")] string? name) =>
        await _messageBus.InvokeAsync<Result<List<MenuCategory>>>(new GetMenuCategoriesQuery(name));

    [TranslateResultToActionResult]
    [HttpGet("{menuCategoryId:guid}")]
    public async Task<Result<MenuCategory>> GetMenuCategoryAsync([FromRoute] Guid menuCategoryId) =>
        await _messageBus.InvokeAsync<Result<MenuCategory>>(new GetMenuCategoryByIdQuery(menuCategoryId));

    [TranslateResultToActionResult]
    [HttpPost]
    [EndpointGroupName("administration")]
    public async Task<Result<MenuCategory>> CreateMenuCategoryAsync([FromBody] CreateMenuCategoryDTO dto) =>
        await _messageBus.InvokeAsync<Result<MenuCategory>>(new CreateMenuCategoryCommand(dto));

    [TranslateResultToActionResult]
    [HttpPatch("{menuCategoryId:guid}")]
    [EndpointGroupName("administration")]
    public async Task<Result<MenuCategory>> UpdateMenuCategoryAsync([FromRoute] Guid menuCategoryId, [FromBody] UpdateMenuCategoryDTO dto) =>
        await _messageBus.InvokeAsync<Result<MenuCategory>>(new UpdateMenuCategoryCommand(menuCategoryId, dto));

    [TranslateResultToActionResult]
    [HttpDelete("{menuCategoryId:guid}")]
    [EndpointGroupName("administration")]
    public async Task<Result> RemoveMenuCategoryAsync([FromRoute] Guid menuCategoryId) =>
        await _messageBus.InvokeAsync<Result>(new RemoveMenuCategoryCommand(menuCategoryId));
}
