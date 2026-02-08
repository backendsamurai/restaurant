namespace Restaurant.Domain;

public interface IAuditable
{
    DateTimeOffset CreatedAtUtc { get; }

    DateTimeOffset UpdatedAtUtc { get; }
}
