using FluentValidation;
using Humanizer;
using Microsoft.Extensions.Logging;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Database;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;
using SystemClaims = System.Security.Claims;

namespace Restaurant.Services.Implementations;

public sealed class AuthService(
    IRepository<Customer> customerRepository,
    IJwtService jwtService,
    IPasswordHasherService passwordHasherService,
    IValidator<LoginCustomerModel> loginCustomerModelValidator,
    ILogger<AuthService> logger,
    Admin admin
) : IAuthService
{
    public async Task<ResultWithObject> LoginCustomerAsync(LoginCustomerModel loginCustomerModel)
    {
        var validationResult = await loginCustomerModelValidator.ValidateAsync(loginCustomerModel);

        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation error\n{@ErrorMsg}", validationResult.Errors.First().ErrorMessage);
            return DetailedError.Invalid("One of field are not valid", validationResult.Errors.First().ErrorMessage);
        }

        var customer = await customerRepository.WhereFirstAsync<Customer>(c => c.Email == loginCustomerModel.Email);

        if (customer == null)
        {
            var notFoundError = DetailedError.NotFound("customer with provided email not found");
            logger.LogWarning("{@Err}", notFoundError);
            return notFoundError;
        }

        if (!passwordHasherService.Verify(loginCustomerModel.Password, customer.PasswordHash))
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("AUTH_WRONG_PASSWORD")
                .WithTitle("Wrong password")
                .WithMessage("Check the password is correct and try again")
            );


        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.Name,customer.Name),
            new (ClaimTypes.Email, customer.Email),
            new (ClaimTypes.UserRole, UserRole.Customer.ToString().Humanize(LetterCasing.LowerCase)),
        ];

        var tokenResult = jwtService.GenerateToken(claims);

        if (tokenResult.IsError)
            return tokenResult.DetailedError!;

        return ResultWithObject.Success(new
        {
            customer.Id,
            customer.Name,
            customer.Email,
            AccessToken = tokenResult.Value
        });
    }

    public ResultWithObject LoginAdmin(LoginAdminModel loginAdminModel)
    {
        var hashedPassword = passwordHasherService.Hash(admin.Password);

        if (!passwordHasherService.Verify(loginAdminModel.Password, hashedPassword))
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("AUTH_WRONG_PASSWORD")
                .WithTitle("Wrong password")
                .WithMessage("Check the password is correct and try again")
            );

        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.UserRole, UserRole.Admin.ToString().Humanize(LetterCasing.LowerCase))
        ];

        var tokenResult = jwtService.GenerateToken(claims);

        if (tokenResult.IsError)
            return tokenResult.DetailedError!;

        return ResultWithObject.Success(new { AccessToken = tokenResult.Value });
    }
}