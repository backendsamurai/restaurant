using Humanizer;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Models.Order;

public sealed class OrderQuery : IQueryObject
{
    public Guid? CustomerId { get; set; }
    public Guid? WaiterId { get; set; }
    public Guid? DeskId { get; set; }
    public OrderStatus? Status { get; set; }

    public void SetQueryValue(string key, string value)
    {
        if (key == "CustomerId") CustomerId = Guid.Parse(value);
        if (key == "Status")
            Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), value.ToString().Dehumanize(), true);
    }
}