using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models;

namespace Restaurant.Application.Auth.LoginAdmin;

public sealed class LoginAdminCommandHandler(IAuthService authService) : IRequestHandler<LoginAdminCommand, ResultWithObject>
{
    public Task<ResultWithObject> Handle(LoginAdminCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(authService.LoginAdmin(new LoginAdminModel(request.Password)));
}