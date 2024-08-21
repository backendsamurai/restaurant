using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.API.Entities;

namespace Restaurant.API.Data.ValueConverters;

public sealed class PaymentStatusValueConverter : ValueConverter<PaymentStatus, string>
{
    public PaymentStatusValueConverter() : base(
        (status) => status.ToString(),
        (value) => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), value))
    { }
}
