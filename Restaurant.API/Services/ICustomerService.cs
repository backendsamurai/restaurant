using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Security.Models;

namespace Restaurant.API.Services;

public interface ICustomerService
{
    public Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id);
    public Task<Result<CustomerResponse>> GetCustomerByEmailAsync(string email);
    public Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest);
    public Task<Result<CustomerResponse>> UpdateCustomerAsync(Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerRequest updateCustomerRequest);
    public Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser);
}
