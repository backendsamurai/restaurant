using Ardalis.Result;
using FluentValidation;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.API.Entities;
using Restaurant.API.Mail.Models;
using EmailTemplatesModels = Restaurant.API.Mail.Templates.Models;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Models.User;
using Ardalis.Result.FluentValidation;

namespace Restaurant.API.Services.Implementations;

public class EmailVerificationService(
    IBus bus,
    IUserRepository userRepository,
    IOtpGeneratorService otpGeneratorService,
    IRedisConnectionProvider redisConnectionProvider,
    IValidator<EmailVerificationModel> emailVerificationValidator
) : IEmailVerificationService
{
    public const int OTP_CODE_LENGTH = 6;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOtpGeneratorService _otpGeneratorService = otpGeneratorService;
    private readonly IRedisConnectionProvider _redisConnectionProvider = redisConnectionProvider;
    private readonly IValidator<EmailVerificationModel> _emailVerificationValidator = emailVerificationValidator;
    private readonly IBus _bus = bus;

    public async Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var user = await _userRepository.SelectByEmail(authenticatedUser.Email).FirstOrDefaultAsync();

        if (user is null)
            return Result.NotFound("user not found");

        return await SendVerificationEmailAsync(user);
    }

    public async Task<Result> SendVerificationEmailAsync(User user)
    {
        if (user.IsVerified)
            return Result.Success();

        var otpCode = _otpGeneratorService.Generate(OTP_CODE_LENGTH);

        var isCached = await _redisConnectionProvider.Connection
            .JsonSetAsync($"otp-{user.Id}", "$", otpCode.ToString(), WhenKey.NotExists, TimeSpan.FromMinutes(2));

        if (!isCached)
            return Result.Error("error while sending verification email");

        await _bus.Publish(Tuple.Create(user, otpCode).Adapt<EmailSendMetadata<EmailTemplatesModels.EmailVerificationModel>>());

        return Result.Success();
    }

    public async Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, EmailVerificationModel emailVerificationModel)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var validationResult = await _emailVerificationValidator.ValidateAsync(emailVerificationModel);

        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var user = await _userRepository.SelectByEmail(authenticatedUser.Email).FirstOrDefaultAsync();

        if (user is null)
            return Result.NotFound("user not found");

        var otpCodeFromCache = await _redisConnectionProvider.Connection.JsonGetAsync<int?>($"otp-{user.Id}");

        if (otpCodeFromCache is null || emailVerificationModel.OtpCode != otpCodeFromCache.ToString())
            return Result.Error("invalid verification code");

        user.IsVerified = true;

        if (await _userRepository.UpdateAsync(user))
            return Result.Success();

        return Result.Error("cannot verify this user");
    }

}
