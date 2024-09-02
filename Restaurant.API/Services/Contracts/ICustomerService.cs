using Ardalis.Result;
using Restaurant.API.Models.Customer;
using Restaurant.API.Security.Models;

namespace Restaurant.API.Services.Contracts;

public interface ICustomerService
{
    public Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id);
    public Task<Result<List<CustomerResponse>>> GetCustomerByEmailAsync(string email);
    public Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerModel createCustomerModel);
    public Task<Result<CustomerResponse>> UpdateCustomerAsync(Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerModel updateCustomerModel);
    public Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser);
}
