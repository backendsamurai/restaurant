namespace Restaurant.API.Types;

public sealed class DetailedError(string code, string type, string message, string detail)
{
    public string Code { get; private set; } = code;
    public string Type { get; private set; } = type;
    public string Message { get; private set; } = message;
    public string Detail { get; private set; } = detail;
}
