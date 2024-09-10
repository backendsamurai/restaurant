using Microsoft.Extensions.Options;
using Restaurant.API.Mail.Configurations;
using Restaurant.API.Mail.Services;

namespace Restaurant.API.Mail;

public static class DependencyInjection
{
    public static IServiceCollection AddMailConfiguration(this IServiceCollection services) =>
        services.ConfigureOptions<MailOptionsSetup>();

    public static IServiceCollection AddMailServices(this IServiceCollection services) =>
        services.AddScoped<IEmailSenderService, EmailSenderService>();

    public static IServiceCollection AddMail(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var mailOptions = serviceProvider.GetRequiredService<IOptions<MailOptions>>()
            ?? throw new InvalidOperationException("mail configuration not found");

        services
            .AddFluentEmail(mailOptions.Value.SenderEmail)
            .AddSmtpSender(
                host: mailOptions.Value.SmtpHost,
                port: mailOptions.Value.SmtpPort,
                username: mailOptions.Value.SmtpUsername,
                password: mailOptions.Value.SmtpPassword
            ).AddRazorRenderer();

        return services;
    }
}
