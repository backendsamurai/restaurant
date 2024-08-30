using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Validators.Helpers;

namespace Restaurant.API.Services;

public sealed class CustomerService(
    ICustomerRepository customerRepository,
    IUserRepository userRepository,
    IValidator<CreateCustomerModel> createCustomerValidator,
    IValidator<UpdateCustomerModel> updateCustomerValidator,
    IPasswordHasher passwordHasher
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<CreateCustomerModel> _createCustomerValidator = createCustomerValidator;
    private readonly IValidator<UpdateCustomerModel> _updateCustomerValidator = updateCustomerValidator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerModel createCustomerModel)
    {
        var validationResult = await _createCustomerValidator.ValidateAsync(createCustomerModel);

        if (validationResult.IsValid)
        {
            var passwordHash = _passwordHasher.Hash(createCustomerModel.Password!);

            var userFromDb = await _userRepository
                .SelectByEmail(createCustomerModel.Email!)
                .ProjectToType<User>()
                .FirstOrDefaultAsync();

            if (userFromDb is not null)
            {
                return Result.Conflict("cannot create customer with the same email address");
            }

            var user = new User
            {
                Name = createCustomerModel.Name!,
                Email = createCustomerModel.Email!,
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

    public async Task<Result<CustomerResponse>> UpdateCustomerAsync(
        Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerModel updateCustomerModel)
    {
        bool isModified = false;
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        if (customer.User.Email != authenticatedUser.Email)
            return Result.Unauthorized();

        if (updateCustomerModel.Name is not null && updateCustomerModel.Name != customer.User.Name)
        {
            var nameValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Name));

            if (!nameValidationResult.IsValid)
                return Result.Invalid(nameValidationResult.AsErrors());

            customer.User.Name = updateCustomerModel.Name;
            isModified = true;
        }

        if (updateCustomerModel.Email is not null && updateCustomerModel.Email != customer.User.Email)
        {
            var emailValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Email));

            if (!emailValidationResult.IsValid)
                return Result.Invalid(emailValidationResult.AsErrors());

            customer.User.Email = updateCustomerModel.Email;
            isModified = true;
        }

        if (updateCustomerModel.Password is not null && !_passwordHasher.Verify(updateCustomerModel.Password, customer.User.PasswordHash))
        {
            var passwordValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Password));

            if (!passwordValidationResult.IsValid)
                return Result.Invalid(passwordValidationResult.AsErrors());

            customer.User.PasswordHash = _passwordHasher.Hash(updateCustomerModel.Password);
            isModified = true;
        }

        if (isModified)
        {
            var isUpdated = await _customerRepository.UpdateAsync(customer.User);
            return isUpdated ? Result.Success(customer.Adapt<CustomerResponse>()) : Result.Error("cannot update customer");
        }

        return Result.Error("don`t need update customer");
    }

    public async Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser)
    {
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound("customer not found");

        if (customer.User.Email != authenticatedUser.Email)
            return Result.Unauthorized();

        var isRemoved = await _customerRepository.RemoveAsync(customer);

        return isRemoved ? Result.Success() : Result.Error("cannot remove customer from database");
    }
}
