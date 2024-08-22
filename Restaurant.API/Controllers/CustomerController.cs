using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
      private readonly ICustomerService _customerService = customerService;

      [TranslateResultToActionResult]
      [ExpectedFailures(ResultStatus.NotFound)]
      [HttpGet("{id:guid}")]
      public async Task<Result<CustomerResponse>> GetCustomerById([FromRoute(Name = "id")] Guid id) =>
            await _customerService.GetCustomerByIdAsync(id);

      [TranslateResultToActionResult]
      [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
      [HttpGet()]
      public async Task<Result<CustomerResponse>> GetCustomerByEmail([FromQuery(Name = "email")] string email) =>
            await _customerService.GetCustomerByEmailAsync(email);

      [TranslateResultToActionResult]
      [ExpectedFailures(ResultStatus.Conflict, ResultStatus.Invalid)]
      [HttpPost]
      public async Task<Result<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest createCustomerRequest) =>
          await _customerService.CreateCustomerAsync(createCustomerRequest);

      [TranslateResultToActionResult]
      [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound, ResultStatus.Error)]
      [HttpPatch("{id:guid}")]
      public async Task<Result<CustomerResponse>> UpdateCustomer(
         [FromRoute(Name = "id")] Guid id,
         [FromBody] UpdateCustomerRequest updateCustomerRequest
      ) => await _customerService.UpdateCustomerAsync(id, updateCustomerRequest);

      [TranslateResultToActionResult]
      [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
      [HttpDelete("{id:guid}")]
      public async Task<Result> RemoveCustomer([FromRoute(Name = "id")] Guid id) =>
            await _customerService.RemoveCustomerAsync(id);
}
