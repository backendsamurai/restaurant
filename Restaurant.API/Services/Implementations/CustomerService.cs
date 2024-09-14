using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Redis.OM.Searching;
using Restaurant.API.Caching.Models;
using Restaurant.API.Entities;
using Restaurant.API.Extensions;
using Restaurant.API.Models.Customer;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class CustomerService(
    ICustomerRepository customerRepository,
    IUserRepository userRepository,
    IValidator<CreateCustomerModel> createCustomerValidator,
    IValidator<UpdateCustomerModel> updateCustomerValidator,
    IPasswordHasherService passwordHasher,
    IRedisCollection<CustomerCacheModel> cache,
    IEmailVerificationService emailVerificationService
) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<CreateCustomerModel> _createCustomerValidator = createCustomerValidator;
    private readonly IValidator<UpdateCustomerModel> _updateCustomerValidator = updateCustomerValidator;
    private readonly IPasswordHasherService _passwordHasher = passwordHasher;
    private readonly IRedisCollection<CustomerCacheModel> _cache = cache;
    private readonly IEmailVerificationService _emailVerificationService = emailVerificationService;

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
                return Result.Conflict(
                    code: "CSR-440-001",
                    type: "entity_already_exists",
                    message: "Customer with this email already exists",
                    detail: "Please check provided email or provide another email address"
                );

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
                var customerResponse = customer.Adapt<CustomerResponse>();

                await _cache.InsertAsync(customerResponse);
                await _emailVerificationService.SendVerificationEmailAsync(customer.User);

                return Result.Created(customerResponse);
            }

            return Result.Error(
                code: "CSR-554-001",
                type: "error_while_creation_customer",
                message: "Cannot create customer",
                detail: "Unexpected error"
            );
        }

        return Result.Invalid(
            code: "CSR-440-002",
            type: "invalid_model",
            message: "One of field are not valid",
            detail: "Check all fields and try again"
        );
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _cache.GetOrSetAsync(c => c.CustomerId == id,
            async () => await _customerRepository.SelectById(id).ProjectToType<CustomerResponse>().FirstOrDefaultAsync());

        return customer is null
            ? Result.NotFound(
                code: "CSR-440-003",
                type: "entity_not_found",
                message: "Customer not found",
                detail: "Please provide correct id"
            ) : Result.Success(customer);
    }

    public async Task<Result<CustomerResponse>> UpdateCustomerAsync(
        Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerModel updateCustomerModel)
    {
        bool isModified = false;
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound(
                code: "CSR-440-003",
                type: "entity_not_found",
                message: "Customer not found",
                detail: "Please provide correct id"
            );

        if (customer.User.Email != authenticatedUser.Email)
            return Result.Unauthorized(
                code: "CSR-554-051",
                type: "missing_authorization",
                message: "Unauthorized",
                detail: "Please provide authentication"
            );

        if (updateCustomerModel.Name is not null && updateCustomerModel.Name != customer.User.Name)
        {
            var nameValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Name));

            if (!nameValidationResult.IsValid)
                return Result.Invalid(
                    code: "CSR-440-002",
                    type: "invalid_model",
                    message: "One of field are not valid",
                    detail: "Check all fields and try again"
                );

            customer.User.Name = updateCustomerModel.Name;
            isModified = true;
        }

        if (updateCustomerModel.Email is not null && updateCustomerModel.Email != customer.User.Email)
        {
            var emailValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Email));

            if (!emailValidationResult.IsValid)
                return Result.Invalid(
                    code: "CSR-440-002",
                    type: "invalid_model",
                    message: "One of field are not valid",
                    detail: "Check all fields and try again"
                );

            customer.User.Email = updateCustomerModel.Email;
            isModified = true;
        }

        if (updateCustomerModel.Password is not null && !_passwordHasher.Verify(updateCustomerModel.Password, customer.User.PasswordHash))
        {
            var passwordValidationResult = await _updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Password));

            if (!passwordValidationResult.IsValid)
                return Result.Invalid(
                   code: "CSR-440-002",
                    type: "invalid_model",
                    message: "One of field are not valid",
                    detail: "Check all fields and try again"
                );

            customer.User.PasswordHash = _passwordHasher.Hash(updateCustomerModel.Password);
            isModified = true;
        }

        if (isModified)
        {
            var isUpdated = await _customerRepository.UpdateAsync(customer.User);

            if (isUpdated)
            {
                var customerResponse = customer.Adapt<CustomerResponse>();
                await _cache.UpdateAsync(customerResponse);

                return Result.Success(customerResponse);
            }

            return Result.Error(
                code: "CSR-554-002",
                type: "error_while_updating_customer",
                message: "Cannot update customer",
                detail: "Unexpected error"
            );
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser)
    {
        var customer = await _customerRepository.SelectById(id).ProjectToType<Customer>().FirstOrDefaultAsync();

        if (customer is null)
            return Result.NotFound(
                code: "CSR-440-003",
                type: "entity_not_found",
                message: "Customer not found",
                detail: "Please provide correct id"
            );

        if (customer.User.Email != authenticatedUser.Email)
            return Result.Unauthorized(
                code: "CSR-554-051",
                type: "missing_authorization",
                message: "Unauthorized",
                detail: "Please provide authentication"
            );

        var isRemoved = await _customerRepository.RemoveAsync(customer);

        if (isRemoved)
        {
            await _cache.DeleteAsync(customer);
            return Result.Success();
        }

        return Result.Error(
            code: "CSR-554-003",
            type: "error_while_removing_customer",
            message: "Cannot remove customer",
            detail: "Unexpected error"
        );
    }
}
