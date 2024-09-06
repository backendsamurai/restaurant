using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.API.Caching.Models;

[Document(IndexName = "customers-idx", StorageType = StorageType.Json, Prefixes = ["customers"])]
public class CustomerCacheModel
{
    [RedisIdField]
    [Indexed(PropertyName = "id")]
    [RedisField(PropertyName = "id")]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [Indexed(PropertyName = "user_id")]
    [RedisField(PropertyName = "user_id")]
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

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
