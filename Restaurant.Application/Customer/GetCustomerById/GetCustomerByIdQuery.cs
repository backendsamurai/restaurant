using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer.GetCustomerById;

public sealed class GetCustomerByIdQuery : IRequest<Result<CustomerResponse>>
{
    public Guid CustomerId { get; set; }
}