using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("desks")]
public sealed class DeskController(IDeskService deskService) : ControllerBase
{
    private readonly IDeskService _deskService = deskService;

    [ApplyResult]
    [HttpGet]
    public async Task<Result<List<Desk>>> GetAllDesks() =>
        await _deskService.GetAllDesksAsync();

    [ApplyResult]
    [HttpGet("{id:guid}")]
    public async Task<Result<Desk>> GetDeskById([FromRoute(Name = "id")] Guid id) =>
        await _deskService.GetDeskByIdAsync(id);

    [ApplyResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPost]
    public async Task<Result<Desk>> CreateDesk([FromBody] CreateDeskModel createDeskModel) =>
        await _deskService.CreateDeskAsync(createDeskModel);

    [ApplyResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<Desk>> UpdateDesk(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateDeskModel updateDeskModel
    ) => await _deskService.UpdateDeskAsync(id, updateDeskModel);

    [ApplyResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveDesk([FromRoute(Name = "id")] Guid id) =>
        await _deskService.RemoveDeskAsync(id);
}
