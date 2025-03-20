using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Customer.RemoveCustomer;

public sealed class RemoveCustomerCommandHandler(ICustomerService customerService) : IRequestHandler<RemoveCustomerCommand, Result>
{
    public async Task<Result> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken) =>
        await customerService.RemoveCustomerAsync(request.CustomerId, request.User);
}