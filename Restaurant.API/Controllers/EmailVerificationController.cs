using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("verification")]
public class EmailVerificationController(IEmailVerificationService emailVerificationService) : ControllerBase
{
    [Authorize]
    [HttpPost("send")]
    public async Task<Result> SendVerification() =>
        await emailVerificationService.SendVerificationEmailAsync(User.Adapt<AuthenticatedUser>());

    [Authorize]
    [HttpPost("check")]
    public async Task<Result> CheckVerification([FromBody] EmailVerificationModel verificationModel) =>
        await emailVerificationService
            .SetVerifiedAsync(User.Adapt<AuthenticatedUser>(), verificationModel);
}