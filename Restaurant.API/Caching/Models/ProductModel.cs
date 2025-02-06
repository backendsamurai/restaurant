using System.Text.Json.Serialization;
using Redis.OM.Modeling;
using Restaurant.Domain;

namespace Restaurant.API.Caching.Models;

[Document(
    IndexName = "products-idx",
    StorageType = StorageType.Json,
    Prefixes = ["products"]
)]
public class ProductModel
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

    [Indexed(PropertyName = "description")]
    [RedisField(PropertyName = "description")]
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [Indexed(PropertyName = "image_url")]
    [RedisField(PropertyName = "image_url")]
    [JsonPropertyName("image_url")]
    public required string ImageUrl { get; set; }

    [Indexed(PropertyName = "price")]
    [RedisField(PropertyName = "price")]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [Indexed(PropertyName = "category")]
    [RedisField(PropertyName = "category")]
    [JsonPropertyName("category")]
    public required ProductCategory Category { get; set; }
}
