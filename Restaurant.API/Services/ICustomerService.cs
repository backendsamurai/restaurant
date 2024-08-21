using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;

namespace Restaurant.API.Services;

public interface ICustomerService
{
    public Task<Result<CustomerResponse>> GetCustomerByIdAsync(Guid id);
    public Task<Result<CustomerResponse>> GetCustomerByEmailAsync(string email);
    public Task<Result<CustomerResponse>> CreateCustomerAsync(CreateCustomerRequest createCustomerRequest);
}
