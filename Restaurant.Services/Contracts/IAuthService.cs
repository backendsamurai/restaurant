using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Services.Contracts;

public interface IAuthService
{
    public Task<ResultWithObject> LoginCustomerAsync(LoginCustomerModel loginCustomerModel);
    public ResultWithObject LoginAdmin(LoginAdminModel loginAdminModel);
}
