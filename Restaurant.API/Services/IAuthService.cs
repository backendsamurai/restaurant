using Ardalis.Result;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Employee;
using Restaurant.API.Models.User;

namespace Restaurant.API.Services;

public interface IAuthService
{
    public Task<Result<LoginCustomerResponse>> LoginCustomerAsync(string audience, LoginUserModel loginUserModel);
    public Task<Result<LoginEmployeeResponse>> LoginEmployeeAsync(string audience, LoginUserModel loginUserModel);
}
