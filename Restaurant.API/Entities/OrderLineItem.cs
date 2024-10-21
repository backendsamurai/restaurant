using Restaurant.API.Entities.Abstractions;

namespace Restaurant.API.Entities;

public sealed class OrderLineItem : Entity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public int Count { get; set; }
}
