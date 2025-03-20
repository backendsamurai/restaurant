using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler(ICustomerService customerService) : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken) =>
        await customerService.UpdateCustomerAsync(request.CustomerId, request.User, request.UpdateCustomerModel);
}