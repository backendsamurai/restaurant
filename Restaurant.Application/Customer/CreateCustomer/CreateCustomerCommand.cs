using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer.CreateCustomer;

public sealed class CreateCustomerCommand : IRequest<Result<CustomerResponse>>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}