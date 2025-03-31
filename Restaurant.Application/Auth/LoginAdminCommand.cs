using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;

namespace Restaurant.Application.Auth;

public sealed record LoginAdminCommand(string Password) : IRequest<ResultWithObject>;

public sealed class LoginAdminCommandHandler(IAuthService authService) : IRequestHandler<LoginAdminCommand, ResultWithObject>
{
    public Task<ResultWithObject> Handle(LoginAdminCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(authService.LoginAdmin(new LoginAdminModel(request.Password)));
}