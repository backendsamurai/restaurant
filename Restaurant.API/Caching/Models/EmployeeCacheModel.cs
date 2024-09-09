using System.Text.Json.Serialization;
using Redis.OM.Modeling;

namespace Restaurant.API.Caching.Models;

[Document(
    IndexName = "employees-idx",
    StorageType = StorageType.Json,
    Prefixes = ["employees"]
)]
public class EmployeeCacheModel
{
    [RedisIdField]
    [Indexed(PropertyName = "employee_id")]
    [RedisField(PropertyName = "employee_id")]
    [JsonPropertyName("employee_id")]
    public Guid EmployeeId { get; set; }

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

    [Indexed(PropertyName = "employee_role")]
    [RedisField(PropertyName = "employee_role")]
    [JsonPropertyName("employee_role")]
    public required string EmployeeRole { get; set; }
}
