using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer;

public sealed record UpdateCustomerCommand(Guid CustomerId, AuthenticatedUser User, UpdateCustomerModel UpdateCustomerModel) : IRequest<Result<CustomerResponse>>;

public sealed class UpdateCustomerCommandHandler(ICustomerService customerService) : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken) =>
        await customerService.UpdateCustomerAsync(request.CustomerId, request.User, request.UpdateCustomerModel);
}