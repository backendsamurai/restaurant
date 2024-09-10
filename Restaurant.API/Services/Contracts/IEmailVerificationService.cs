using Ardalis.Result;
using Restaurant.API.Entities;
using Restaurant.API.Security.Models;

namespace Restaurant.API.Services.Contracts;

public interface IEmailVerificationService
{
    public Task<Result> SendVerificationEmailAsync(User user);
    public Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser);
    public Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, string otpCode);
}
