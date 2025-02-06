using FluentValidation;
using Mapster;
using MassTransit;
using Redis.OM;
using Redis.OM.Contracts;
using Restaurant.API.Mail.Models;
using EmailTemplatesModels = Restaurant.API.Mail.Templates.Models;
using Restaurant.API.Security.Models;
using Restaurant.API.Security.Services.Contracts;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using Restaurant.API.Repositories;
using Restaurant.Domain;
using Restaurant.API.Models;

namespace Restaurant.API.Services.Implementations;

public class EmailVerificationService(
    IBus bus,
    IRepository<Customer> customerRepository,
    IOtpGeneratorService otpGeneratorService,
    IRedisConnectionProvider redisConnectionProvider,
    IValidator<EmailVerificationModel> emailVerificationValidator
) : IEmailVerificationService
{
    public const int OtpCodeLength = 6;

    public async Task<Result> SendVerificationEmailAsync(AuthenticatedUser authenticatedUser)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == authenticatedUser.Email);

        if (customer is null)
            return DetailedError.NotFound("Customer cannot find in storage");

        return await SendVerificationEmailAsync(customer);
    }

    public async Task<Result> SendVerificationEmailAsync(Customer customer)
    {
        if (customer.IsVerified)
            return Result.Success();

        var otpCode = otpGeneratorService.Generate(OtpCodeLength);

        var isCached = await redisConnectionProvider.Connection
            .JsonSetAsync($"otp-{customer.Id}", "$", otpCode.ToString(), WhenKey.NotExists, TimeSpan.FromMinutes(2));

        if (!isCached)
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("SEND_VERIFICATION_MAIL_PROBLEM")
                .WithTitle("Cannot send verification email")
                .WithMessage("Try again later")
            );

        await bus.Publish(Tuple.Create(customer, otpCode).Adapt<EmailSendMetadata<EmailTemplatesModels.EmailVerificationModel>>());

        return Result.Success();
    }

    public async Task<Result> SetVerifiedAsync(AuthenticatedUser authenticatedUser, EmailVerificationModel emailVerificationModel)
    {
        if (authenticatedUser.IsVerified)
            return Result.Success();

        var validationResult = await emailVerificationValidator.ValidateAsync(emailVerificationModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field is invalid. Check provided data and try again later");

        var customer = await customerRepository.FirstOrDefaultAsync(c => c.Email == authenticatedUser.Email);

        if (customer is null)
            return DetailedError.NotFound("Customer cannot find in storage");

        var otpCodeFromCache = await redisConnectionProvider.Connection.JsonGetAsync<int?>($"otp-{customer.Id}");

        if (otpCodeFromCache is null || emailVerificationModel.OtpCode != otpCodeFromCache.ToString())
            return DetailedError.Create(b => b
                .WithStatus(ResultStatus.Error)
                .WithSeverity(ErrorSeverity.Error)
                .WithType("INVALID_OTP_CODE")
                .WithTitle("Invalid OTP code")
                .WithMessage("Please provide correct OTP Code or try send code again later")
            );

        customer.SetVerified(true);

        if (await customerRepository.UpdateAsync(customer))
            return Result.Success();

        return DetailedError.Unexpected("Please check all provided data and try again.If doesn`t help please contact support");
    }

}
