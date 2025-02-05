namespace Restaurant.API.Types;

public sealed class DetailedErrorBuilder
{
    private readonly DetailedError _detailedError = DetailedError.Empty();

    public DetailedErrorBuilder WithStatus(int status)
    {
        _detailedError.Status = status;
        return this;
    }

    public DetailedErrorBuilder WithSeverity(ErrorSeverity severity)
    {
        _detailedError.Severity = severity;
        return this;
    }

    public DetailedErrorBuilder WithType(string type)
    {
        _detailedError.Type = type;
        return this;
    }

    public DetailedErrorBuilder WithTitle(string title)
    {
        _detailedError.Title = title;
        return this;
    }

    public DetailedErrorBuilder WithTitle(string template, params object[] args)
    {
        _detailedError.Title = string.Format(template, args);
        return this;
    }

    public DetailedErrorBuilder WithMessage(string message)
    {
        _detailedError.Message = message;
        return this;
    }

    public DetailedErrorBuilder WithMessage(string template, params object[] args)
    {
        _detailedError.Message = string.Format(template, args);
        return this;
    }

    public DetailedError Build() => _detailedError;
}