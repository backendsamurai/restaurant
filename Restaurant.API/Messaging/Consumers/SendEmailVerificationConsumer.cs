using MassTransit;
using Restaurant.API.Mail.Services;
using Restaurant.API.Mail.Models;
using Restaurant.API.Mail.Templates.Models;

namespace Restaurant.API.Messaging.Consumers;

public sealed class SendEmailVerificationConsumer : IConsumer<EmailSendMetadata<EmailVerificationModel>>
{
    private readonly bool _simulate = false;
    private readonly ILogger<SendEmailVerificationConsumer> _logger;
    private readonly IEmailSenderService _emailSenderService;

    public SendEmailVerificationConsumer(
        ILogger<SendEmailVerificationConsumer> logger,
        IEmailSenderService emailSenderService, IHostEnvironment environment)
    {
        _logger = logger;
        _emailSenderService = emailSenderService;

        if (environment.IsDevelopment())
            _simulate = true;
    }

    public async Task Consume(ConsumeContext<EmailSendMetadata<EmailVerificationModel>> context)
    {
        if (_simulate)
            await SimulateSendEmail(context.Message);
        else
            await SendVerificationMail(context.Message);
    }

    private async Task SimulateSendEmail(EmailSendMetadata<EmailVerificationModel> metadata)
    {
        _logger.LogInformation("Sending verification mail to : {recipient}", metadata.RecipientEmail);

        await Task.Delay(TimeSpan.FromSeconds(5));
    }

    private async Task SendVerificationMail(EmailSendMetadata<EmailVerificationModel> metadata)
    {
        _logger.LogInformation("Sending verification mail");

        try
        {
            var response = await _emailSenderService.SendAsync(metadata);

            if (!response.Successful)
                _logger.LogError("Error when sending verification mail: {ErrorMessage}", response.ErrorMessages.First());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot send verification mail");
        }
    }
}
