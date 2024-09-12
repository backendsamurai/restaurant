using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models.User;
using Restaurant.API.Security.Models;
using Restaurant.API.Services.Contracts;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("verification")]
public class EmailVerificationController(IEmailVerificationService emailVerificationService) : ControllerBase
{
    private readonly IEmailVerificationService _emailVerificationService = emailVerificationService;

    [Authorize]
    [HttpPost("send")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
    public async Task<Result> SendVerification() =>
        await _emailVerificationService.SendVerificationEmailAsync(User.Adapt<AuthenticatedUser>());

    [Authorize]
    [HttpPost("check")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound, ResultStatus.Error)]
    public async Task<Result> CheckVerification([FromBody] EmailVerificationModel verificationModel) =>
        await _emailVerificationService
            .SetVerifiedAsync(User.Adapt<AuthenticatedUser>(), verificationModel);
}