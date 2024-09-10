using Ardalis.Result;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.API.Entities;
using Restaurant.API.Mail.Models;
using Restaurant.API.Mail.Templates.Models;
using Restaurant.API.Repositories;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using StackExchange.Redis;

namespace Restaurant.API.Services.Implementations;

public class EmailVerificationService(
    IUserRepository userRepository,
    IOtpGeneratorService otpGeneratorService,
    IRedisConnectionProvider redisConnectionProvider,
    IBus bus
) : IEmailVerificationService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IOtpGeneratorService _otpGeneratorService = otpGeneratorService;
    private readonly IRedisConnectionProvider _redisConnectionProvider = redisConnectionProvider;
    private readonly IBus _bus = bus;

    public async Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser)
    {
        var user = await _userRepository.SelectByEmail(authenticatedUser.Email).FirstOrDefaultAsync();

        if (user is null)
            return Result.NotFound("user not found");

        return await SendVerificationEmailAsync(user);
    }

    public async Task<Result> SendVerificationEmailAsync(User user)
    {
        var otpCode = _otpGeneratorService.Generate();

        var isCached = await _redisConnectionProvider.Connection
            .JsonSetAsync($"otp-{user.Id}", "$", otpCode, TimeSpan.FromMinutes(2));

        if (!isCached)
            return Result.Error("error while sending verification email");

        await _bus.Publish(Tuple.Create(user, otpCode).Adapt<EmailSendMetadata<EmailVerificationModel>>());

        return Result.Success();
    }

    public async Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, string otpCode)
    {
        var user = await _userRepository.SelectByEmail(authenticatedUser.Email).FirstOrDefaultAsync();

        if (user is null)
            return Result.NotFound("user not found");

        var otpCodeFromCache = await _redisConnectionProvider.Connection.JsonGetAsync<string>($"otp-{user.Id}");

        if (otpCodeFromCache is null || otpCode != otpCodeFromCache.ToString())
            return Result.Error("invalid verification code");

        user.IsVerified = true;

        if (await _userRepository.UpdateAsync(user))
            return Result.Success();

        return Result.Error("cannot verify this user");
    }

}
