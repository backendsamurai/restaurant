using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Dto.Requests;
using Restaurant.API.Dto.Responses;
using Restaurant.API.Repositories;
using Restaurant.API.Services;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("customers")]
public sealed class CustomerController(ICustomerRepository customerRepository, ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerService _customerService = customerService;

    [HttpGet("{id:guid}")]
    public async Task<CustomerResponse?> GetCustomerById([FromRoute(Name = "id")] Guid id)
    {
        var customerQuery = _customerRepository.SelectById(id);
        var customer = await customerQuery
            .ProjectToType<CustomerResponse?>()
            .FirstOrDefaultAsync();

        return customer;
    }

    [HttpGet("{email}")]
    public async Task<string> GetCustomerByEmail([FromRoute(Name = "email")] string email)
    {
        return await Task.FromResult(email);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest createCustomerRequest)
    {
        var customerResponse = await _customerService.CreateCustomerAsync(createCustomerRequest);

        if (customerResponse is not null)
        {
            return Ok(customerResponse);
        }

        return BadRequest(new { Message = "Validation error ocurred" });
    }
}
