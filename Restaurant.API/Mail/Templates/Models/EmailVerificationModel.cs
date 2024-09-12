namespace Restaurant.API.Mail.Templates.Models;

public sealed class EmailVerificationModel(Guid userId, string userName, string otpCode)
{
    public Guid UserId { get; set; } = userId;
    public string UserName { get; set; } = userName;
    public string OtpCode { get; set; } = otpCode;
}
