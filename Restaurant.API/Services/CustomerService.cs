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
    IValidator<CreateCustomerRequest> createCustomerValidator,
    IValidator<UpdateCustomerRequest> updateCustomerValidator,
    IPasswordHasher passwordHasher
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<CreateCustomerRequest> _createCustomerValidator = createCustomerValidator;
    private readonly IValidator<UpdateCustomerRequest> _updateCustomerValidator = updateCustomerValidator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest)
    {
        var validationResult = await _createCustomerValidator.ValidateAsync(createCustomerRequest);

        if (validationResult.IsValid)
        {
            var passwordHash = _passwordHasher.Hash(createCustomerRequest.Password!);

            var userFromDb = await _userRepository.SelectByEmailAsync(createCustomerRequest.Email!);

            if (userFromDb is not null)
            {
                return Result.Conflict("cannot create customer with the same email address");
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
                ? Result.NotFound("customer not found")
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
            ? Result.NotFound("customer not found")
            : Result.Success(customer);
    }

    public async Task<Result<CustomerResponse>> UpdateCustomerAsync(Guid id, UpdateCustomerRequest updateCustomerRequest)
    {
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        if (updateCustomerRequest.Name is not null && updateCustomerRequest.Name != customer.User.Name)
        {
            var nameValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerRequest, options => options.IncludeProperties(u => u.Name));

            if (!nameValidationResult.IsValid)
                return Result.Invalid(nameValidationResult.AsErrors());

            customer.User.Name = updateCustomerRequest.Name;
        }

        if (updateCustomerRequest.Email is not null && updateCustomerRequest.Email != customer.User.Email)
        {
            var emailValidationResult = await _updateCustomerValidator
            .ValidateAsync(updateCustomerRequest, options => options.IncludeProperties(u => u.Email));

            if (!emailValidationResult.IsValid)
                return Result.Invalid(emailValidationResult.AsErrors());

            customer.User.Email = updateCustomerRequest.Email;
        }

        if (updateCustomerRequest.Password is not null && !_passwordHasher.Verify(updateCustomerRequest.Password, customer.User.PasswordHash))
        {
            var passwordValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerRequest, options => options.IncludeProperties(u => u.Password));

            if (!passwordValidationResult.IsValid)
                return Result.Invalid(passwordValidationResult.AsErrors());

            customer.User.PasswordHash = _passwordHasher.Hash(updateCustomerRequest.Password);
        }

        var isUpdated = await _customerRepository.UpdateAsync(customer.User);

        return isUpdated ? Result.Success(customer.Adapt<CustomerResponse>()) : Result.Error("cannot update customer");
    }

    public async Task<Result> RemoveCustomerAsync(Guid id)
    {
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        var isRemoved = await _customerRepository.RemoveAsync(customer);

        return isRemoved ? Result.Success() : Result.Error("cannot remove customer from database");
    }
}
