using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Services.Implementations;

public sealed class CustomerService(
    ICustomerRepository customerRepository,
    IUserRepository userRepository,
    IValidator<CreateCustomerModel> createCustomerValidator,
    IValidator<UpdateCustomerModel> updateCustomerValidator,
    IPasswordHasherService passwordHasher,
    IRedisCollection<CustomerCacheModel> cache
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<CreateCustomerModel> _createCustomerValidator = createCustomerValidator;
    private readonly IValidator<UpdateCustomerModel> _updateCustomerValidator = updateCustomerValidator;
    private readonly IPasswordHasherService _passwordHasher = passwordHasher;
    private readonly IRedisCollection<CustomerCacheModel> _cache = cache;

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
                return Result.Conflict("customer with this email already exists");
            }

            var user = new User
            {
                Name = createCustomerModel.Name!,
                Email = createCustomerModel.Email!,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);
            var customer = await _customerRepository.AddAsync(user);

            if (customer is not null)
            {
                var response = customer.Adapt<CustomerResponse>();
                await _cache.InsertAsync(response.Adapt<CustomerCacheModel>());
                return Result.Success(response);
            }

            return Result.Error("cannot create customer");
        }

        return Result.Invalid(validationResult.AsErrors());
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id)
    {
        var cachedCustomer = await _cache.FirstOrDefaultAsync(c => c.Id == id);

        if (cachedCustomer is not null)
            return Result.Success(cachedCustomer.Adapt<CustomerResponse>());

        var customer = await _customerRepository.SelectById(id).ProjectToType<CustomerResponse>().FirstOrDefaultAsync();

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

            if (isUpdated)
            {
                var response = customer.Adapt<CustomerResponse>();
                await _cache.UpdateAsync(response.Adapt<CustomerCacheModel>());
                return Result.Success(response);
            }

            return Result.Error("cannot update customer");
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

        if (isRemoved)
        {
            await _cache.DeleteAsync(customer.Adapt<CustomerCacheModel>());
            return Result.Success();
        }

        return Result.Error("cannot remove customer from database");
    }
}
