using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Services.Contracts;

public interface ICustomerService
{
    public Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id);
    public Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerModel createCustomerModel);
    public Task<Result<CustomerResponse>> UpdateCustomerAsync(Guid id, AuthenticatedUser authenticatedUser, UpdateCustomerModel updateCustomerModel);
    public Task<Result> RemoveCustomerAsync(Guid id, AuthenticatedUser authenticatedUser);
}
