using System.Text.Json.Serialization;

namespace Restaurant.API.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Customer, Employee
}
