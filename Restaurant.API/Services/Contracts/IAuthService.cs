using Ardalis.Result;
using Restaurant.API.Models.User;

namespace Restaurant.API.Services.Contracts;

public interface IAuthService
{
    public Task<Result<LoginUserResponse>> LoginCustomerAsync(string audience, LoginUserModel loginUserModel);
    public Task<Result<LoginUserResponse>> LoginEmployeeAsync(string audience, LoginUserModel loginUserModel);
}
