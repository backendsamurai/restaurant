using Restaurant.API.Entities;
using Restaurant.API.Models.User;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Contracts;

public interface IAuthService
{
    public Task<Result<LoginUserResponse>> LoginUserAsync(string audience, UserRole userRole, LoginUserModel loginUserModel);
}
