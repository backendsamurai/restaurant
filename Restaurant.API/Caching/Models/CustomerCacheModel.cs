using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.API.Caching.Models;

[Document(
    IndexName = "customers-idx",
    StorageType = StorageType.Json,
    Prefixes = ["customers"]
)]
public class CustomerCacheModel
{
    [RedisIdField]
    [Indexed(PropertyName = "customer_id")]
    [RedisField(PropertyName = "customer_id")]
    [JsonPropertyName("customer_id")]
    public Guid CustomerId { get; set; }

    [Indexed(PropertyName = "user_id")]
    [RedisField(PropertyName = "user_id")]
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [Indexed(PropertyName = "user_name")]
    [RedisField(PropertyName = "user_name")]
    [JsonPropertyName("user_name")]
    public required string UserName { get; set; }

    [Indexed(PropertyName = "user_email")]
    [RedisField(PropertyName = "user_email")]
    [JsonPropertyName("user_email")]
    public required string UserEmail { get; set; }

    [Indexed(PropertyName = "is_verified")]
    [RedisField(PropertyName = "is_verified")]
    [JsonPropertyName("is_verified")]
    public bool IsVerified { get; set; }
}
