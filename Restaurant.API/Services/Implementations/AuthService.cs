using FluentValidation;
using Humanizer;
using Mapster;
using Restaurant.API.Entities;
using Restaurant.API.Models.User;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using SystemClaims = System.Security.Claims;

namespace Restaurant.API.Services.Implementations;

public sealed record UserInfo(User User, Guid Id, string EmployeeRole);

public sealed class AuthService(
    IRepository<Customer> customerRepository,
    IRepository<Employee> employeeRepository,
    IJwtService jwtService,
    IPasswordHasherService passwordHasherService,
    IValidator<LoginUserModel> loginUserValidator,
    ILogger<AuthService> logger
) : IAuthService
{
    private readonly IRepository<Customer> _customerRepository = customerRepository;
    private readonly IRepository<Employee> _employeeRepository = employeeRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
    private readonly IValidator<LoginUserModel> _loginUserValidator = loginUserValidator;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<LoginUserResponse>> LoginUserAsync(UserRole userRole, LoginUserModel loginUserModel)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(loginUserModel);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation error\n{@ErrorMsg}", validationResult.Errors[0].ErrorMessage);

            return Result.Invalid(
                code: "ATZ-000-001",
                type: "invalid_model",
                message: "One of field are not valid",
                detail: "Check all fields and try again"
            );
        }

        var info = userRole switch
        {
            UserRole.Customer => await GetCustomerInfo(loginUserModel.Email!),
            UserRole.Employee => await GetEmployeeInfo(loginUserModel.Email!),
            _ => null
        };

        if (info is null)
        {
            string msg = userRole == UserRole.Customer ? "Customer not found" : "Employee not found";
            _logger.LogWarning("{@msg}", msg);

            return Result.NotFound(
                code: "ATZ-000-002",
                type: "entity_not_found",
                message: msg,
                detail: "Please provide correct id"
            );
        }

        if (!_passwordHasherService.Verify(loginUserModel.Password!, info.User.PasswordHash))
            return Result.Error(
                code: "ATZ-100-001",
                type: "auth_wrong_password",
                message: "Wrong password",
                detail: "Please check your password is correct"
            );

        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.Name,info.User.Name),
            new (ClaimTypes.Email, info.User.Email),
            new (ClaimTypes.UserRole, info.User.Role.ToString().Humanize(LetterCasing.LowerCase)),
            new (ClaimTypes.IsVerified, info.User.IsVerified.ToString().Humanize(LetterCasing.LowerCase))
        ];

        if (userRole == UserRole.Employee && !string.IsNullOrEmpty(info.EmployeeRole))
            claims.Add(new(ClaimTypes.EmployeeRole, info.EmployeeRole));

        var tokenResult = _jwtService.GenerateToken(claims);

        if (tokenResult.IsError)
            return Result.Error(tokenResult.DetailedError!);

        return Result.Success(
            Tuple.Create(info.User, info.Id, tokenResult.Value, info.EmployeeRole)
                .Adapt<LoginUserResponse>());
    }

    private async Task<UserInfo?> GetCustomerInfo(string email) =>
        await _customerRepository.WhereFirstAsync<UserInfo?>(c => c.User.Email == email);

    private async Task<UserInfo?> GetEmployeeInfo(string email) =>
        await _employeeRepository.WhereFirstAsync<UserInfo?>(e => e.User.Email == email);
}