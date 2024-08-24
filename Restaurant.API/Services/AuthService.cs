using System.Security.Claims;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Entities;
using Restaurant.API.Repositories;

namespace Restaurant.API.Services;

public sealed class AuthService(
    ICustomerRepository customerRepository,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    IValidator<LoginUserRequest> loginUserValidator
) : IAuthService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IJwtService _jwtService = jwtService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IValidator<LoginUserRequest> _loginUserValidator = loginUserValidator;

    public async Task<Result<LoginCustomerResponse>> LoginCustomerAsync(string audience, LoginUserRequest loginUserRequest)
    {
        var validationResult = await _loginUserValidator.ValidateAsync(loginUserRequest);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var customer = await _customerRepository
            .SelectByEmail(loginUserRequest.Email!)
            .ProjectToType<Customer>()
            .FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        if (!_passwordHasher.Verify(loginUserRequest.Password!, customer.User.PasswordHash))
            return Result.Error("wrong password");

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name,customer.User.Name, ClaimValueTypes.String),
            new (ClaimTypes.Email, customer.User.Email, ClaimValueTypes.Email)
        };

        var tokenResult = _jwtService.GenerateToken(audience, claims);

        if (tokenResult.IsError())
            return Result.Error(tokenResult.Errors.First());

        return Result.Success(
            Tuple.Create(customer, tokenResult.Value)
                .Adapt<LoginCustomerResponse>()
        );
    }
}
