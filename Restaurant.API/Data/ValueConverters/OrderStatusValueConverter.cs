using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.ValueConverters;

public sealed class OrderStatusValueConverter : ValueConverter<OrderStatus, string>
{
    public OrderStatusValueConverter() : base(
        (status) => status.ToString().ToLower(),
        (value) => (OrderStatus)Enum.Parse(typeof(OrderStatus), value.ToUpper()))
    { }
}
