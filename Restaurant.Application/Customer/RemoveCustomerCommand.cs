using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;

namespace Restaurant.Application.Customer;

public sealed record RemoveCustomerCommand(Guid CustomerId, AuthenticatedUser User) : IRequest<Result>;

public sealed class RemoveCustomerCommandHandler(ICustomerService customerService) : IRequestHandler<RemoveCustomerCommand, Result>
{
    public async Task<Result> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken) =>
        await customerService.RemoveCustomerAsync(request.CustomerId, request.User);
}