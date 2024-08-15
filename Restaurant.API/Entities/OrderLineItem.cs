namespace Restaurant.API.Entities;

public sealed class OrderLineItem : Entity
{
    public required Product Product { get; set; }
    public int Count { get; set; }
}
