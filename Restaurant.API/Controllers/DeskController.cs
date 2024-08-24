using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Entities;
using Restaurant.API.Services;

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
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Conflict, ResultStatus.Error)]
    [HttpPost]
    public async Task<Result<Desk>> CreateDesk([FromBody] CreateDeskRequest createDeskRequest) =>
        await _deskService.CreateDeskAsync(createDeskRequest);

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound, ResultStatus.Error)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<Desk>> UpdateDesk(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateDeskRequest updateDeskRequest
    ) => await _deskService.UpdateDeskAsync(id, updateDeskRequest);

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveDesk([FromRoute(Name = "id")] Guid id) =>
        await _deskService.RemoveDeskAsync(id);
}
