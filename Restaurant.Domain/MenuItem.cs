namespace Restaurant.Domain;

public sealed class MenuItem : Entity<Guid>, IAuditable, ISoftDeletable
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string ImageUrl { get; private set; }

    public decimal Price { get; private set; }

    public MenuCategory Category { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAtUtc { get; private set; }

    private MenuItem()
    {
        Name = default!;
        Description = default!;
        ImageUrl = default!;
        Category = default!;
    }

    public MenuItem(
        Guid id,
        string name,
        string description,
        string imageUrl,
        decimal price,
        MenuCategory category
    ) : base(id)
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = price;
        Category = category;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void ChangeName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
    }

    public void ChangeDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            Description = description;
    }

    public void ChangePrice(decimal? price)
    {
        if (price is not null && price.HasValue)
            Price = price.GetValueOrDefault();
    }

    public void ChangeCategory(MenuCategory? category)
    {
        if (category is not null)
            Category = category;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTimeOffset.UtcNow;
    }
}
