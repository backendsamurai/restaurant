using Microsoft.Extensions.Options;

namespace Restaurant.API.Mail.Configurations;

public sealed class MailOptions
{
    public required string SenderEmail { get; set; }
    public required string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public required string SmtpUsername { get; set; }
    public required string SmtpPassword { get; set; }
}

public sealed class MailOptionsSetup(IConfiguration configuration) : IConfigureOptions<MailOptions>
{
    private const string SectionName = "Mail";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(MailOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}