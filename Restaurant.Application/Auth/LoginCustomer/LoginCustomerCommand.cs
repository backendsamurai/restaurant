using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Auth.LoginCustomer;

public sealed class LoginCustomerCommand : IRequest<ResultWithObject>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}