using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.API.Caching.Models;

[Document(
    IndexName = "employees-roles-idx",
    StorageType = StorageType.Json,
    Prefixes = ["employee-roles"]
)]
public class EmployeeRoleCacheModel
{
    [RedisIdField]
    [RedisField(PropertyName = "id")]
    [Indexed(PropertyName = "id")]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [RedisField(PropertyName = "name")]
    [Indexed(PropertyName = "name")]
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}
