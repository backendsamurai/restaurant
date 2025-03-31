using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Auth;
using Restaurant.Shared.Common;

namespace Restaurant.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("customer")]
        public async Task<ResultWithObject> LoginCustomer([FromBody] LoginCustomerCommand command) => await mediator.Send(command);

        [HttpPost("admin")]
        public async Task<ResultWithObject> LoginAdmin([FromBody] LoginAdminCommand command) => await mediator.Send(command);
    }
}