using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;

namespace Restaurant.API.Services;

public interface ICustomerService
{
    public Task<CustomerResponse?> GetCustomerByIdAsync(Guid id);
    public Task<CustomerResponse?> GetCustomerByEmailAsync(string email);
    public Task<CustomerResponse?> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest);
}
