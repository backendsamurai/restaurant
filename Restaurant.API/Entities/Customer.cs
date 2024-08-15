namespace Restaurant.API.Entities;

public sealed class Customer : Entity
{
    public required User User { get; set; }
}
