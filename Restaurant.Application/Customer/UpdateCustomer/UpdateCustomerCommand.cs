using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer.UpdateCustomer;

public sealed class UpdateCustomerCommand : IRequest<Result<CustomerResponse>>
{
    public Guid CustomerId { get; set; }
    public required AuthenticatedUser User { get; set; }
    public required UpdateCustomerModel UpdateCustomerModel { get; set; }
}