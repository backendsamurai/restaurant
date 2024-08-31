using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Models.Employee;
using Restaurant.API.Repositories;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("employees")]
public sealed class EmployeeController(
    IEmployeeService employeeService,
    IEmployeeRepository employeeRepository
) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    [TranslateResultToActionResult]
    [HttpGet]
    public async Task<Result<List<EmployeeResponse>>> GetEmployees(
        [FromQuery(Name = "email")] string? email, [FromQuery(Name = "role")] string? role)
    {
        if (!string.IsNullOrEmpty(email))
            return await _employeeService.GetEmployeeByEmailAsync(email);

        if (!string.IsNullOrEmpty(role))
            return await _employeeService.GetEmployeeByRoleAsync(role);

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(role))
        {
            return await _employeeRepository
                .SelectAll()
                .Where(e => e.User.Email.Contains(email) && e.Role.Name.Contains(role))
                .ProjectToType<EmployeeResponse>()
                .ToListAsync();
        }

        return await _employeeService.GetAllEmployeesAsync();
    }

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<Result<EmployeeResponse>> GetEmployeeById([FromRoute(Name = "id")] Guid id) =>
        await _employeeService.GetEmployeeByIdAsync(id);

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Conflict, ResultStatus.Error, ResultStatus.Invalid)]
    [HttpPost]
    public async Task<Result<EmployeeResponse>> CreateEmployee([FromBody] CreateEmployeeModel createEmployeeModel) =>
        await _employeeService.CreateEmployeeAsync(createEmployeeModel);

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid, ResultStatus.NotFound)]
    [HttpPatch("{id:guid}")]
    public async Task<Result<EmployeeResponse>> UpdateEmployee(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateEmployeeModel updateEmployeeModel
    ) => await _employeeService.UpdateEmployeeAsync(id, updateEmployeeModel);

    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound)]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveEmployee([FromRoute(Name = "id")] Guid id) =>
        await _employeeService.RemoveEmployeeAsync(id);
}