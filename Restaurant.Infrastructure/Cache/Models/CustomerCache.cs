using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.Infrastructure.Cache.Models;

[Document(
    IndexName = "customers-idx",
    StorageType = StorageType.Json,
    Prefixes = ["customers"]
)]
public class CustomerCache
{
    [RedisIdField]
    [Indexed(PropertyName = "id")]
    [RedisField(PropertyName = "id")]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Indexed(PropertyName = "name")]
    [RedisField(PropertyName = "name")]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [Indexed(PropertyName = "email")]
    [RedisField(PropertyName = "email")]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [Indexed(PropertyName = "is_verified")]
    [RedisField(PropertyName = "is_verified")]
    [JsonPropertyName("is_verified")]
    public bool IsVerified { get; set; }
}
