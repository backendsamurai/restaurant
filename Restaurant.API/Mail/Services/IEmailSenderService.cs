using FluentEmail.Core.Models;
using Restaurant.API.Mail.Models;

namespace Restaurant.API.Mail.Services;

public interface IEmailSenderService
{
    public SendResponse Send<T>(EmailSendMetadata<T> metadata) where T : class;
    public Task<SendResponse> SendAsync<T>(EmailSendMetadata<T> metadata) where T : class;
}
