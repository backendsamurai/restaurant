using Humanizer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Domain;

namespace Restaurant.API.Data.ValueConverters;

public sealed class OrderStatusValueConverter : ValueConverter<OrderStatus, string>
{
    public OrderStatusValueConverter() : base(
        (status) => status.ToString().Underscore(),
        (value) => (OrderStatus)Enum.Parse(typeof(OrderStatus), value.Dehumanize()))
    { }
}
