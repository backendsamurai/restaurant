namespace Restaurant.Domain;

public sealed class MenuCategory : Entity<Guid>, IAuditable, ISoftDeletable
{
    public string Name { get; private set; } = default!;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAtUtc { get; private set; }

    public MenuCategory(Guid id, string name) : base(id)
    {
        Name = name;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void ChangeName(string name) => Name = name;

    public void MarkDeleted()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTimeOffset.UtcNow;
    }
}
