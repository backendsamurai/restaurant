using MassTransit;
using Restaurant.API.Mail.Services;
using Restaurant.API.Mail.Models;
using Restaurant.API.Mail.Templates.Models;

namespace Restaurant.API.Messaging.Consumers;

public sealed class SendEmailVerificationConsumer(
    ILogger<SendEmailVerificationConsumer> logger,
    IEmailSenderService emailSenderService
) : IConsumer<EmailSendMetadata<EmailVerificationModel>>
{
    private readonly ILogger<SendEmailVerificationConsumer> _logger = logger;
    private readonly IEmailSenderService _emailSenderService = emailSenderService;

    public async Task Consume(ConsumeContext<EmailSendMetadata<EmailVerificationModel>> context)
    {
        _logger.LogInformation("Sending verification mail");

        try
        {
            var response = await _emailSenderService.SendAsync(context.Message);

            if (!response.Successful)
                _logger.LogError("Error when sending verification mail: {ErrorMessage}", response.ErrorMessages.First());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot send verification mail");
        }
    }
}
