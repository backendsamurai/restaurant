using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Entities;
using Restaurant.API.Models.Desk;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("desks")]
public sealed class DeskController(IDeskService deskService) : ControllerBase
{
    private readonly IDeskService _deskService = deskService;

    [TranslateResultToActionResult]
    [HttpGet]
    public async Task<Result<List<Desk>>> GetAllDesks() =>
        await _deskService.GetAllDesksAsync();

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<Result<Desk>> GetDeskById([FromRoute(Name = "id")] Guid id)
        => await _deskService.GetDeskByIdAsync(id);

    [TranslateResultToActionResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Conflict, ResultStatus.Error)]
    [HttpPost]
    public async Task<Result<Desk>> CreateDesk([FromBody] CreateDeskModel createDeskModel) =>
        await _deskService.CreateDeskAsync(createDeskModel);

    [TranslateResultToActionResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound, ResultStatus.Error)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<Desk>> UpdateDesk(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateDeskModel updateDeskModel
    ) => await _deskService.UpdateDeskAsync(id, updateDeskModel);

    [TranslateResultToActionResult]
    [Authorize(AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveDesk([FromRoute(Name = "id")] Guid id) =>
        await _deskService.RemoveDeskAsync(id);
}
