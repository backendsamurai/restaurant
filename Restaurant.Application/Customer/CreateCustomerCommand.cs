using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Customer;

public sealed record CreateCustomerCommand(string Name, string Email, string Password) : IRequest<Result<CustomerResponse>>;

public sealed class CreateCustomerCommandHandler(ICustomerService customerService)
     : IRequestHandler<CreateCustomerCommand, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken) =>
        await customerService.CreateCustomerAsync(new CreateCustomerModel(request.Name, request.Email, request.Password));
}