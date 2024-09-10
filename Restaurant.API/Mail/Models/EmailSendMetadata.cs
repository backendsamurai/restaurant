namespace Restaurant.API.Mail.Models;

public sealed class EmailSendMetadata<T> where T : class
{
    public required string RecipientEmail { get; set; }
    public required string Subject { get; set; }
    public required string TemplateFileName { get; set; }
    public T? TemplateModel { get; set; }
}
