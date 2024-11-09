using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Restaurant.API.Attributes;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Entities;
using Restaurant.API.Models.Employee;
using Restaurant.API.Models.Order;
using Restaurant.API.Models.User;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("employees")]
public sealed class EmployeeController(
    IEmployeeService employeeService,
    IAuthService authService,
    IOrderService orderService,
    IRepository<Employee> employeeRepository,
    IOptions<JwtOptions> jwtOptions
) : ControllerBase
{
    private readonly IEmployeeService _employeeService = employeeService;
    private readonly IAuthService _authService = authService;
    private readonly IOrderService _orderService = orderService;
    private readonly IRepository<Employee> _employeeRepository = employeeRepository;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [ServiceFilter<ApplyResultAttribute>]
    [HttpGet]
    public async Task<Result<List<EmployeeResponse>>> GetEmployees(
        [FromQuery(Name = "email")] string? email, [FromQuery(Name = "role")] string? role)
    {
        if (!string.IsNullOrEmpty(email))
        {
            var validationResult = QueryValidationHelper.Validate(email);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await _employeeService.GetEmployeeByEmailAsync(email);
        }

        if (!string.IsNullOrEmpty(role))
        {
            var validationResult = QueryValidationHelper.Validate(role);

            if (validationResult.IsError)
                return validationResult.DetailedError!;

            return await _employeeService.GetEmployeeByRoleAsync(role);
        }

        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(role))
        {
            return await _employeeRepository
                .WhereAsync<EmployeeResponse>(e => e.User.Email.Contains(email) && e.Role.Name.Contains(role));
        }

        return await _employeeService.GetAllEmployeesAsync();
    }

    [ServiceFilter<ApplyResultAttribute>]
    [HttpGet("{id:guid}")]
    public async Task<Result<EmployeeResponse>> GetEmployeeById([FromRoute(Name = "id")] Guid id) =>
        await _employeeService.GetEmployeeByIdAsync(id);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpPost]
    public async Task<Result<EmployeeResponse>> CreateEmployee([FromBody] CreateEmployeeModel createEmployeeModel) =>
        await _employeeService.CreateEmployeeAsync(createEmployeeModel);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpPatch("{id:guid}")]
    public async Task<Result<EmployeeResponse>> UpdateEmployee(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateEmployeeModel updateEmployeeModel
    ) => await _employeeService.UpdateEmployeeAsync(id, updateEmployeeModel);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpDelete("{id:guid}")]
    public async Task<Result> RemoveEmployee([FromRoute(Name = "id")] Guid id) =>
        await _employeeService.RemoveEmployeeAsync(id);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpGet("{employeeId:guid}/orders")]
    public async Task<Result<List<OrderResponse>>> GetOrdersByEmployee([FromRoute(Name = "employeeId")] Guid employeeId) =>
        await _orderService.GetOrdersByEmployeeAsync(employeeId);

    [ServiceFilter<ApplyResultAttribute>]
    [HttpPost("authentication")]
    public async Task<Result<LoginUserResponse>> LoginEmployee([FromBody] LoginUserModel loginUserModel)
    {
        var audienceDetectResult = DetectAudienceHeaderHelper.Detect(Request.Headers, _jwtOptions);

        if (audienceDetectResult.IsError)
            return audienceDetectResult.DetailedError!;

        return await _authService.LoginUserAsync(audienceDetectResult.Value!, UserRole.Employee, loginUserModel);
    }
}