using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Customer;

namespace Restaurant.Application.Auth.LoginCustomer;

public sealed class LoginCustomerCommandHandler(IAuthService authService) : IRequestHandler<LoginCustomerCommand, ResultWithObject>
{
    public async Task<ResultWithObject> Handle(LoginCustomerCommand request, CancellationToken cancellationToken) =>
        await authService.LoginCustomerAsync(new LoginCustomerModel(request.Email, request.Password));
}