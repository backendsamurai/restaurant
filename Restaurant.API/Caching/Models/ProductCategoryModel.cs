using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.API.Caching.Models;

[Document(
    IndexName = "product-categories-idx",
    StorageType = StorageType.Json,
    Prefixes = ["product-categories"]
)]
public class ProductCategoryModel
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
}
