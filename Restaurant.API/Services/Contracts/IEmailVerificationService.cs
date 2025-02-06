using Restaurant.API.Models;
using Restaurant.API.Security.Models;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Services.Contracts;

public interface IEmailVerificationService
{
    public Task<Result> SendVerificationEmailAsync(Customer customer);
    public Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser);
    public Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, EmailVerificationModel emailVerificationModel);
}
