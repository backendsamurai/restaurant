using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Domain;

namespace Restaurant.Persistence.ValueConverters;

public sealed class OrderStatusValueConverter : ValueConverter<OrderStatus, string>
{
    public OrderStatusValueConverter() : base(
        (status) => status.ToString(),
        (value) => (OrderStatus)Enum.Parse(typeof(OrderStatus), value))
    { }
}
