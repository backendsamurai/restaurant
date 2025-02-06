using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models;
using Restaurant.API.Models.Customer;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("customer")]
        public async Task<ResultWithObject> LoginCustomer([FromBody] LoginCustomerModel model) =>
             await authService.LoginCustomerAsync(model);

        [HttpPost("admin")]
        public ResultWithObject LoginAdmin([FromBody] LoginAdminModel model) => authService.LoginAdmin(model);
    }
}