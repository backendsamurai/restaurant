using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Humanizer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;
using Restaurant.API.Models.User;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using SystemClaims = System.Security.Claims;

namespace Restaurant.API.Services.Implementations;

public sealed class AuthService(
    ICustomerRepository customerRepository,
    IEmployeeRepository employeeRepository,
    IJwtService jwtService,
    IPasswordHasherService passwordHasherService,
    IValidator<LoginUserModel> loginUserValidator
) : IAuthService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;
    private readonly IValidator<LoginUserModel> _loginUserValidator = loginUserValidator;

    public async Task<Result<LoginUserResponse>> LoginCustomerAsync(string audience, LoginUserModel loginUserModel)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(loginUserModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var customer = await _customerRepository
            .SelectByEmail(loginUserModel.Email!)
            .ProjectToType<Customer>()
            .FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        if (!_passwordHasherService.Verify(loginUserModel.Password!, customer.User.PasswordHash))
            return Result.Error("wrong password");

        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.Name, customer.User.Name),
            new (ClaimTypes.Email, customer.User.Email),
            new (ClaimTypes.UserRole, customer.User.Role.ToString().Humanize(LetterCasing.LowerCase))
        ];

        var tokenResult = _jwtService.GenerateToken(audience, claims);

        if (tokenResult.IsError())
            return Result.Error(tokenResult.Errors.First());

        return Result.Success(
            Tuple.Create(customer.User, customer.Id, tokenResult.Value, string.Empty)
                .Adapt<LoginUserResponse>()
        );
    }

    public async Task<Result<LoginUserResponse>> LoginEmployeeAsync(string audience, LoginUserModel loginUserModel)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(loginUserModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var employee = await _employeeRepository
            .SelectByEmail(loginUserModel.Email!)
            .ProjectToType<Employee>()
            .FirstOrDefaultAsync();

        if (employee is null)
            return Result.NotFound("employee not found");

        if (!_passwordHasherService.Verify(loginUserModel.Password!, employee.User.PasswordHash))
            return Result.Error("wrong password");

        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.Name, employee.User.Name),
            new (ClaimTypes.Email, employee.User.Email),
            new (ClaimTypes.UserRole, employee.User.Role.ToString().Humanize(LetterCasing.LowerCase)),
            new (ClaimTypes.EmployeeRole, employee.Role.Name)
        ];

        var tokenResult = _jwtService.GenerateToken(audience, claims);

        if (tokenResult.IsError())
            return Result.Error(tokenResult.Errors.First());

        return Result.Success(
            Tuple.Create(employee.User, employee.Id, tokenResult.Value, employee.Role.Name)
            .Adapt<LoginUserResponse>());
    }
}
