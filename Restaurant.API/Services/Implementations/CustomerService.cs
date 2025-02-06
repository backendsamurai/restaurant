using FluentValidation;
using Mapster;
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
    IRepository<Customer> customerRepository,
    IRepository<User> userRepository,
    IValidator<CreateCustomerModel> createCustomerValidator,
    IValidator<UpdateCustomerModel> updateCustomerValidator,
    IPasswordHasherService passwordHasher,
    IRedisCollection<CustomerCacheModel> cache,
    IEmailVerificationService emailVerificationService
) : ICustomerService
{
    public async Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerModel createCustomerModel)
    {
        var validationResult = await createCustomerValidator.ValidateAsync(createCustomerModel);

        if (validationResult.IsValid)
        {
            var passwordHash = passwordHasher.Hash(createCustomerModel.Password!);

            var userFromDb = await userRepository.FirstOrDefaultAsync(u => u.Email == createCustomerModel.Email!);

            if (userFromDb is not null)
                return DetailedError.Conflict(
                    "Customer with this email already exists",
                    "Please check provided email or provide another email address"
                );

            var user = new User
            {
                Name = createCustomerModel.Name!,
                Email = createCustomerModel.Email!,
                PasswordHash = passwordHash
            };

            await userRepository.AddAsync(user);

            var customer = await customerRepository.AddAsync(new Customer { User = user });

            if (customer is not null)
            {
                var customerResponse = customer.Adapt<CustomerResponse>();

                await cache.InsertAsync(customerResponse);
                await emailVerificationService.SendVerificationEmailAsync(customer.User);

                return Result.Created(customerResponse);
            }

            return DetailedError.CreatingProblem("Cannot create customer", "Unexpected error");
        }

        return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await cache.GetOrSetAsync(c => c.CustomerId == id,
            async () => await customerRepository.WhereFirstAsync<CustomerResponse>(c => c.Id == id));

        return customer is null ? DetailedError.NotFound("Please provide correct id") : Result.Success(customer);
    }

    public async Task<Result<CustomerResponse>> UpdateCustomerAsync(
        Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerModel updateCustomerModel)
    {
        bool isModified = false;

        var customer = await customerRepository.SelectByIdAsync(id);

        if (customer is null)
            return DetailedError.NotFound("Please provide correct id");

        if (customer.User.Email != authenticatedUser.Email)
            return DetailedError.Unauthorized("Please provide authorization first");

        if (updateCustomerModel.Name is not null && updateCustomerModel.Name != customer.User.Name)
        {
            var nameValidationResult = await updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Name));

            if (!nameValidationResult.IsValid)
                return DetailedError.Invalid("Invalid name", nameValidationResult.Errors.First().ErrorMessage);

            customer.User.Name = updateCustomerModel.Name;
            isModified = true;
        }

        if (updateCustomerModel.Email is not null && updateCustomerModel.Email != customer.User.Email)
        {
            var emailValidationResult = await updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Email));

            if (!emailValidationResult.IsValid)
                return DetailedError.Invalid("Invalid email", emailValidationResult.Errors.First().ErrorMessage);

            customer.User.Email = updateCustomerModel.Email;
            isModified = true;
        }

        if (updateCustomerModel.Password is not null && !passwordHasher.Verify(updateCustomerModel.Password, customer.User.PasswordHash))
        {
            var passwordValidationResult = await updateCustomerValidator
                .ValidateAsync(updateCustomerModel, options => options.IncludeProperties(u => u.Password));

            if (!passwordValidationResult.IsValid)
                return DetailedError.Invalid("Invalid password", passwordValidationResult.Errors.First().ErrorMessage);

            customer.User.PasswordHash = passwordHasher.Hash(updateCustomerModel.Password);
            isModified = true;
        }

        if (isModified)
        {
            var isUpdated = await customerRepository.UpdateAsync(customer);

            if (isUpdated)
            {
                var customerResponse = customer.Adapt<CustomerResponse>();
                await cache.UpdateAsync(customerResponse);

                return Result.Success(customerResponse);
            }

            return DetailedError.UpdatingProblem("Cannot update customer", "Unexpected error");
        }

        return Result.NoContent();
    }

    public async Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser)
    {
        var customer = await customerRepository.SelectByIdAsync(id);

        if (customer is null)
            return DetailedError.NotFound("Please provide correct id");

        if (customer.User.Email != authenticatedUser.Email)
            return DetailedError.Unauthorized("Please provide authorization first");

        var isRemoved = await customerRepository.RemoveAsync(customer);

        if (isRemoved)
        {
            await cache.DeleteAsync(customer);
            return Result.Success();
        }

        return DetailedError.UpdatingProblem("Cannot remove customer", "Unexpected error");
    }
}
