using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Restaurant.API.Attributes;
using Restaurant.API.Controllers.Helpers;
using Restaurant.API.Entities;
using Restaurant.API.Models.Customer;
using Restaurant.API.Models.User;
using Restaurant.API.Security.Configurations;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController(
      ICustomerService customerService,
      IAuthService authService,
      IOptions<JwtOptions> jwtOptions
) : ControllerBase
{
      private readonly ICustomerService _customerService = customerService;
      private readonly IAuthService _authService = authService;
      private readonly JwtOptions _jwtOptions = jwtOptions.Value;

      [ApplyResult]
      [HttpGet("{id:guid}")]
      public async Task<Result<CustomerResponse>> GetCustomerById([FromRoute(Name = "id")] Guid id) =>
            await _customerService.GetCustomerByIdAsync(id);

      [ApplyResult]
      [HttpPost]
      public async Task<Result<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerModel createCustomerModel) =>
          await _customerService.CreateCustomerAsync(createCustomerModel);

      [ApplyResult]
      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpPatch("{id:guid}")]
      public async Task<Result<CustomerResponse>> UpdateCustomer(
         [FromRoute(Name = "id")] Guid id,
         [FromBody] UpdateCustomerModel updateCustomerModel) =>
            await _customerService.UpdateCustomerAsync(id, User.Adapt<AuthenticatedUser>(), updateCustomerModel);

      [ApplyResult]
      [Authorize(AuthorizationPolicies.RequireCustomer)]
      [HttpDelete("{id:guid}")]
      public async Task<Result> RemoveCustomer([FromRoute(Name = "id")] Guid id) =>
            await _customerService.RemoveCustomerAsync(id, User.Adapt<AuthenticatedUser>());

      [ApplyResult]
      [HttpPost("authentication")]
      public async Task<Result<LoginUserResponse>> LoginCustomer([FromBody] LoginUserModel loginUserModel)
      {
            var audienceDetectResult = DetectAudienceHeaderHelper.Detect(Request.Headers, _jwtOptions);

            if (audienceDetectResult.IsError)
                  return Result.Error(audienceDetectResult.DetailedError!);

            return await _authService.LoginUserAsync(audienceDetectResult.Value!, UserRole.Customer, loginUserModel);
      }
}
