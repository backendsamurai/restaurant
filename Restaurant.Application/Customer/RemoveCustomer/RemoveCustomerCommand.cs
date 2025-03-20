using MediatR;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;

namespace Restaurant.Application.Customer.RemoveCustomer;

public sealed class RemoveCustomerCommand : IRequest<Result>
{
    public Guid CustomerId { get; set; }
    public required AuthenticatedUser User { get; set; }
}