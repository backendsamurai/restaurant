using Restaurant.API.Entities;
using Restaurant.API.Models.User;
using Restaurant.API.Security.Models;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Contracts;

public interface IEmailVerificationService
{
    public Task<Result> SendVerificationEmailAsync(User user);
    public Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser);
    public Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, EmailVerificationModel emailVerificationModel);
}
