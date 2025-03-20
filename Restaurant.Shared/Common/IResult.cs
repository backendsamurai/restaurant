namespace Restaurant.Shared.Common;

public interface IResult
{
    public int Status { get; }
    public object? GetValue();
    public DetailedError? GetDetailedError();
}
