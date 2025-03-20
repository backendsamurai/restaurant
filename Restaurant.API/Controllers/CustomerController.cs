using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Customer.CreateCustomer;
using Restaurant.Application.Customer.GetCustomerById;
using Restaurant.Application.Customer.GetCustomerOrders;
using Restaurant.Application.Customer.RemoveCustomer;
using Restaurant.Application.Customer.UpdateCustomer;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;
using Restaurant.Shared.Models.Order;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController(IMediator mediator) : ControllerBase
{
      [HttpGet("{id:guid}")]
      public async Task<Result<CustomerResponse>> GetCustomerById([FromRoute(Name = "id")] Guid customerId) =>
            await mediator.Send(new GetCustomerByIdQuery { CustomerId = customerId });

      [HttpPost]
      public async Task<Result<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerCommand command) => await mediator.Send(command);

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpPatch("{id:guid}")]
      public async Task<Result<CustomerResponse>> UpdateCustomer(
         [FromRoute(Name = "id")] Guid id,
         [FromBody] UpdateCustomerModel updateCustomerModel) =>
            await mediator.Send(new UpdateCustomerCommand
            {
                  CustomerId = id,
                  User = User.Adapt<AuthenticatedUser>(),
                  UpdateCustomerModel = updateCustomerModel
            });

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpDelete("{id:guid}")]
      public async Task<Result> RemoveCustomer([FromRoute(Name = "id")] Guid id) =>
            await mediator.Send(new RemoveCustomerCommand { CustomerId = id, User = User.Adapt<AuthenticatedUser>() });

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpGet("{customerId:guid}/orders")]
      public async Task<Result<List<OrderResponse>>> GetOrders([FromRoute(Name = "customerId")] Guid customerId) =>
            await mediator.Send(new GetCustomerOrdersQuery { CustomerId = customerId });
}
