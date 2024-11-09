using FluentValidation;
using Mapster;
using MassTransit;
using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.API.Entities;
using Restaurant.API.Mail.Models;
using EmailTemplatesModels = Restaurant.API.Mail.Templates.Models;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Models.User;
using Restaurant.API.Types;
using Restaurant.API.Repositories;

namespace Restaurant.API.Services.Implementations;

public class EmailVerificationService(
    IBus bus,
    IRepository<User> userRepository,
    IOtpGeneratorService otpGeneratorService,
    IRedisConnectionProvider redisConnectionProvider,
    IValidator<EmailVerificationModel> emailVerificationValidator
) : IEmailVerificationService
{
    public const int OtpCodeLength = 6;
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IOtpGeneratorService _otpGeneratorService = otpGeneratorService;
    private readonly IRedisConnectionProvider _redisConnectionProvider = redisConnectionProvider;
    private readonly IValidator<EmailVerificationModel> _emailVerificationValidator = emailVerificationValidator;
    private readonly IBus _bus = bus;

    public async Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == authenticatedUser.Email);

        if (user is null)
            return DetailedError.NotFound("User cannot find in storage");

        return await SendVerificationEmailAsync(user);
    }

    public async Task<Result> SendVerificationEmailAsync(User user)
    {
        if (user.IsVerified)
            return Result.Success();

        var otpCode = _otpGeneratorService.Generate(OtpCodeLength);

        var isCached = await _redisConnectionProvider.Connection
            .JsonSetAsync($"otp-{user.Id}", "$", otpCode.ToString(), WhenKey.NotExists, TimeSpan.FromMinutes(2));

        if (!isCached)
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("SEND_VERIFICATION_MAIL_PROBLEM")
                .WithTitle("Cannot send verification email")
                .WithMessage("Try again later")
            );

        await _bus.Publish(Tuple.Create(user, otpCode).Adapt<EmailSendMetadata<EmailTemplatesModels.EmailVerificationModel>>());

        return Result.Success();
    }

    public async Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, EmailVerificationModel emailVerificationModel)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var validationResult = await _emailVerificationValidator.ValidateAsync(emailVerificationModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field is invalid. Check provided data and try again later");

        var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == authenticatedUser.Email);

        if (user is null)
            return DetailedError.NotFound("User cannot find in storage");

        var otpCodeFromCache = await _redisConnectionProvider.Connection.JsonGetAsync<int?>($"otp-{user.Id}");

        if (otpCodeFromCache is null || emailVerificationModel.OtpCode != otpCodeFromCache.ToString())
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("INVALID_OTP_CODE")
                .WithTitle("Invalid OTP code")
                .WithMessage("Please provide correct OTP Code or try send code again later")
            );

        user.IsVerified = true;

        if (await _userRepository.UpdateAsync(user))
            return Result.Success();

        return DetailedError.Unexpected("Please check all provided data and try again.If doesn`t help please contact support");
    }

}
