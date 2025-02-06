using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.Order;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController(
      ICustomerService customerService,
      IOrderService orderService
) : ControllerBase
{
      [HttpGet("{id:guid}")]
      public async Task<Result<CustomerResponse>> GetCustomerById([FromRoute(Name = "id")] Guid id) =>
            await customerService.GetCustomerByIdAsync(id);

      [HttpPost]
      public async Task<Result<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerModel createCustomerModel) =>
          await customerService.CreateCustomerAsync(createCustomerModel);

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpPatch("{id:guid}")]
      public async Task<Result<CustomerResponse>> UpdateCustomer(
         [FromRoute(Name = "id")] Guid id,
         [FromBody] UpdateCustomerModel updateCustomerModel) =>
            await customerService.UpdateCustomerAsync(id, User.Adapt<AuthenticatedUser>(), updateCustomerModel);

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpDelete("{id:guid}")]
      public async Task<Result> RemoveCustomer([FromRoute(Name = "id")] Guid id) =>
            await customerService.RemoveCustomerAsync(id, User.Adapt<AuthenticatedUser>());

      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpGet("{customerId:guid}/orders")]
      public async Task<Result<List<OrderResponse>>> GetOrders([FromRoute(Name = "customerId")] Guid customerId) =>
            await orderService.GetOrdersByCustomerAsync(customerId);
}
