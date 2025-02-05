using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Entities;
using Restaurant.API.Models.EmployeeRole;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("employees/roles")]
public class EmployeeRoleController(
    IEmployeeRoleService employeeRoleService
) : ControllerBase
{
    private readonly IEmployeeRoleService _employeeRoleService = employeeRoleService;

    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpGet]
    public async Task<Result<List<EmployeeRole>>> GetAllRoles([FromQuery(Name = "name")] string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            var validationResult = QueryValidationHelper.Validate(name);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await _employeeRoleService.GetEmployeeRoleByNameAsync(name);
        }

        return await _employeeRoleService.GetAllEmployeeRolesAsync();
    }

    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpGet("{id:guid}")]
    public async Task<Result<EmployeeRole>> GetRoleById([FromRoute(Name = "id")] Guid id) =>
        await _employeeRoleService.GetEmployeeRoleByIdAsync(id);

    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPost]
    public async Task<Result<EmployeeRole>> CreateRole(
        [FromBody] CreateEmployeeRoleModel createEmployeeRoleModel
    ) => await _employeeRoleService.CreateEmployeeRoleAsync(createEmployeeRoleModel);

    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<EmployeeRole>> UpdateRole(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateEmployeeRoleModel updateEmployeeRoleModel
    ) => await _employeeRoleService.UpdateEmployeeRoleAsync(id, updateEmployeeRoleModel);

    [Authorize(Policy = AuthorizationPolicies.RequireEmployeeManager)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveRole([FromRoute(Name = "id")] Guid id) =>
        await _employeeRoleService.RemoveEmployeeRoleAsync(id);
}
