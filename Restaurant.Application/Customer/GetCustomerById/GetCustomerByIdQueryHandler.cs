using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler(ICustomerService customerService) : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken) =>
       await customerService.GetCustomerByIdAsync(request.CustomerId);
}