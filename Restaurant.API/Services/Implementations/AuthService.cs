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
    public async Task<Result<LoginUserResponse>> LoginUserAsync(UserRole userRole, LoginUserModel loginUserModel)
    {
        var validationResult = await loginUserValidator.ValidateAsync(loginUserModel);

        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validation error\n{@ErrorMsg}", validationResult.Errors.First().ErrorMessage);
            return DetailedError.Invalid("One of field are not valid", validationResult.Errors.First().ErrorMessage);
        }

        var info = userRole switch
        {
            UserRole.Customer => await GetCustomerInfo(loginUserModel.Email!),
            UserRole.Employee => await GetEmployeeInfo(loginUserModel.Email!),
            _ => null
        };

        if (info is null)
        {
            var notFoundError = DetailedError.NotFound($"{userRole.Humanize(LetterCasing.Title)} with provided email not found");
            logger.LogWarning("{@Err}", notFoundError);
            return notFoundError;
        }

        if (!passwordHasherService.Verify(loginUserModel.Password!, info.User.PasswordHash))
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("AUTH_WRONG_PASSWORD")
                .WithTitle("Wrong password")
                .WithMessage("Check the password is correct and try again")
            );


        List<SystemClaims.Claim> claims = [
            new (ClaimTypes.Name,info.User.Name),
            new (ClaimTypes.Email, info.User.Email),
            new (ClaimTypes.UserRole, info.User.Role.ToString().Humanize(LetterCasing.LowerCase)),
            new (ClaimTypes.IsVerified, info.User.IsVerified.ToString().Humanize(LetterCasing.LowerCase))
        ];

        if (userRole == UserRole.Employee && !string.IsNullOrEmpty(info.EmployeeRole))
            claims.Add(new(ClaimTypes.EmployeeRole, info.EmployeeRole));

        var tokenResult = jwtService.GenerateToken(claims);

        if (tokenResult.IsError)
            return tokenResult.DetailedError!;

        return Result.Success(
            Tuple.Create(info.User, info.Id, tokenResult.Value, info.EmployeeRole)
                .Adapt<LoginUserResponse>());
    }

    private async Task<UserInfo?> GetCustomerInfo(string email) =>
        await customerRepository.WhereFirstAsync<UserInfo?>(c => c.User.Email == email);

    private async Task<UserInfo?> GetEmployeeInfo(string email) =>
        await employeeRepository.WhereFirstAsync<UserInfo?>(e => e.User.Email == email);
}