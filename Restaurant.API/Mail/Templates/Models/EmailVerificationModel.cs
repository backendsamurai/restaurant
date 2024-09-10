namespace Restaurant.API.Mail.Templates.Models;

public sealed class EmailVerificationModel
{
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string OtpCode { get; set; }
}
