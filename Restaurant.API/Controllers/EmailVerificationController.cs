using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Attributes;
using Restaurant.API.Models.User;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("verification")]
public class EmailVerificationController(IEmailVerificationService emailVerificationService) : ControllerBase
{
    private readonly IEmailVerificationService _emailVerificationService = emailVerificationService;

    [ApplyResult]
    [Authorize]
    [HttpPost("send")]
    public async Task<Result> SendVerification() =>
        await _emailVerificationService.SendVerificationEmailAsync(User.Adapt<AuthenticatedUser>());

    [ApplyResult]
    [Authorize]
    [HttpPost("check")]
    public async Task<Result> CheckVerification([FromBody] EmailVerificationModel verificationModel) =>
        await _emailVerificationService
            .SetVerifiedAsync(User.Adapt<AuthenticatedUser>(), verificationModel);
}