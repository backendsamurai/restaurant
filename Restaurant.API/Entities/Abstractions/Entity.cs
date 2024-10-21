using System.Text.Json.Serialization;

namespace Restaurant.API.Entities.Abstractions;

public abstract class Entity
{
    [JsonPropertyOrder(-1)]
    public Guid Id { get; set; }
}
