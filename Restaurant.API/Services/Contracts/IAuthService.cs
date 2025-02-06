using Restaurant.API.Models;
using Restaurant.API.Models.Customer;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Contracts;

public interface IAuthService
{
    public Task<ResultWithObject> LoginCustomerAsync(LoginCustomerModel loginCustomerModel);
    public ResultWithObject LoginAdmin(LoginAdminModel loginAdminModel);
}
