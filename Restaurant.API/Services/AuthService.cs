using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Humanizer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Employee;
using Restaurant.API.Models.User;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using SystemClaims = System.Security.Claims;

namespace Restaurant.API.Services;

public sealed class AuthService(
    ICustomerRepository customerRepository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    IValidator<LoginUserModel> loginUserValidator
) : IAuthService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidator<LoginUserModel> _loginUserValidator = loginUserValidator;

    public async Task<Result<LoginCustomerResponse>> LoginCustomerAsync(string audience, LoginUserModel loginUserModel)
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

        if (!_passwordHasher.Verify(loginUserModel.Password!, customer.User.PasswordHash))
            return Result.Error("wrong password");

        var claims = new List<SystemClaims.Claim>
        {
            new (ClaimTypes.Name, customer.User.Name),
            new (ClaimTypes.Email, customer.User.Email),
            new (ClaimTypes.UserRole, customer.User.Role.ToString().Humanize(LetterCasing.LowerCase))
        };

        var tokenResult = _jwtService.GenerateToken(audience, claims);

        if (tokenResult.IsError())
            return Result.Error(tokenResult.Errors.First());

        return Result.Success(
            Tuple.Create(customer, tokenResult.Value)
                .Adapt<LoginCustomerResponse>()
        );
    }

    public Task<Result<LoginEmployeeResponse>> LoginEmployeeAsync(string audience, LoginUserModel loginUserModel)
    {
        throw new NotImplementedException();
    }
}
