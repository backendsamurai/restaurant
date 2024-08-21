using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Entities;
using Restaurant.API.Repositories;
using Restaurant.API.Validators.Helpers;

namespace Restaurant.API.Services;

public sealed class CustomerService(
    ICustomerRepository customerRepository,
    IUserRepository userRepository,
    IValidator<CreateCustomerRequest> validator,
    IPasswordHasher passwordHasher
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<CreateCustomerRequest> _validator = validator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest)
    {
        var validationResult = await _validator.ValidateAsync(createCustomerRequest);

        if (validationResult.IsValid)
        {
            var passwordHash = _passwordHasher.Hash(createCustomerRequest.Password!);

            var userFromDb = await _userRepository.SelectByEmailAsync(createCustomerRequest.Email!);

            if (userFromDb is not null)
            {
                return Result.Conflict(["cannot create customer with the same email address"]);
            }

            var user = new User
            {
                Name = createCustomerRequest.Name!,
                Email = createCustomerRequest.Email!,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);
            var customer = await _customerRepository.AddAsync(user);

            return Result.Success(customer.Adapt<CustomerResponse>());
        }

        return Result.Invalid(validationResult.AsErrors());
    }

    public async Task<Result<CustomerResponse>> GetCustomerByEmailAsync(string email)
    {
        if (EmailValidatorHelper.IsEmailValid(email))
        {
            var customer = await _customerRepository
                .SelectByEmail(email)
                .ProjectToType<CustomerResponse>()
                .FirstOrDefaultAsync();

            return customer is null
                ? Result.NotFound(["customer not found"])
                : Result.Success(customer);
        }

        return Result.Error("invalid format of email address");
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepository
            .SelectById(id)
            .ProjectToType<CustomerResponse>()
            .FirstOrDefaultAsync();

        return customer is null
            ? Result.NotFound(["customer not found"])
            : Result.Success(customer);
    }
}
