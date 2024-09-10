using FluentEmail.Core;
using FluentEmail.Core.Models;
using Restaurant.API.Mail.Models;

namespace Restaurant.API.Mail.Services;

public sealed class EmailSenderService(IFluentEmail fluentEmail) : IEmailSenderService
{
    private readonly string _templateRootFolder = $"{Directory.GetCurrentDirectory()}/Mail/Templates/";
    private readonly IFluentEmail _fluentEmail = fluentEmail;

    public SendResponse Send<T>(EmailSendMetadata<T> metadata) where T : class =>
        _fluentEmail
            .To(metadata.RecipientEmail)
            .Subject(metadata.Subject)
            .UsingTemplateFromFile(_templateRootFolder + metadata.TemplateFileName, metadata.TemplateModel)
            .Send();

    public async Task<SendResponse> SendAsync<T>(EmailSendMetadata<T> metadata) where T : class =>
       await _fluentEmail
            .To(metadata.RecipientEmail)
            .Subject(metadata.Subject)
            .UsingTemplateFromFile(_templateRootFolder + metadata.TemplateFileName, metadata.TemplateModel)
            .SendAsync();
}
