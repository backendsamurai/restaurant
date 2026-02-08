using System.Text.Json.Serialization;

namespace Restaurant.Domain;

public abstract class Entity<TId>
{
    [JsonPropertyOrder(-1)]
    public TId Id { get; private set; }

    protected Entity(TId id) : this()
    {
        Id = id;
    }

    // Required by EF Core
    protected Entity()
    {
        Id = default!;
    }
}
