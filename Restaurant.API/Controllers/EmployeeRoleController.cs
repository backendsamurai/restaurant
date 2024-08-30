using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Entities;
using Restaurant.API.Models.EmployeeRole;
using Restaurant.API.Security.Models;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("employees/roles")]
public class EmployeeRoleController(
    IEmployeeRoleService employeeRoleService
) : ControllerBase
{
    private readonly IEmployeeRoleService _employeeRoleService = employeeRoleService;

    [TranslateResultToActionResult]
    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpGet]
    public async Task<Result<List<EmployeeRole>>> GetAllRoles([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
            return await _employeeRoleService.GetEmployeeRoleByNameAsync(name);

        return await _employeeRoleService.GetAllEmployeeRolesAsync();
    }

    [TranslateResultToActionResult]
    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<Result<EmployeeRole>> GetRoleById([FromRoute(Name = "id")] Guid id) =>
        await _employeeRoleService.GetEmployeeRoleByIdAsync(id);

    [TranslateResultToActionResult]
    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Error, ResultStatus.Conflict)]
    [HttpPost]
    public async Task<Result<EmployeeRole>> CreateRole(
        [FromBody] CreateEmployeeRoleModel createEmployeeRoleModel
    ) => await _employeeRoleService.CreateEmployeeRoleAsync(createEmployeeRoleModel);

    [TranslateResultToActionResult]
    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound, ResultStatus.Error)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<EmployeeRole>> UpdateRole(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateEmployeeRoleModel updateEmployeeRoleModel
    ) => await _employeeRoleService.UpdateEmployeeRoleAsync(id, updateEmployeeRoleModel);

    [TranslateResultToActionResult]
    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveRole([FromRoute(Name = "id")] Guid id) =>
        await _employeeRoleService.RemoveEmployeeRoleAsync(id);
}
