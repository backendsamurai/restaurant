using Restaurant.API.Entities.Abstractions;

namespace Restaurant.API.Entities;

public sealed class ProductCategory : Entity
{
    public required string Name { get; set; }
}
