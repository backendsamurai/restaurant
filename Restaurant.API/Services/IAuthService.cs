using Ardalis.Result;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;

namespace Restaurant.API.Services;

public interface IAuthService
{
    public Task<Result<LoginCustomerResponse>> LoginCustomerAsync(string audience, LoginUserRequest loginUserRequest);
}
