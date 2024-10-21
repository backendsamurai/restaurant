using System.Text.Json.Serialization;

namespace Restaurant.API.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    AwaitPayment, Pending, Closed
}
