using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Auth.LoginAdmin;

public sealed class LoginAdminCommand : IRequest<ResultWithObject>
{
    public required string Password { get; set; }
}