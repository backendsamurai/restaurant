namespace Restaurant.API.Types;

public interface IResult
{
    public int Status { get; }
    public object? GetValue();
    public DetailedError? GetDetailedError();
}
