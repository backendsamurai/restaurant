namespace Restaurant.Domain;

public sealed class OrderLineItem : Entity<Guid>
{
    public Guid MenuItemId { get; private set; }

    public MenuItem? MenuItem { get; private set; }

    public int Count { get; private set; }

    public decimal Price => MenuItem?.Price * Count ?? 0;

    public OrderLineItem(Guid id, Guid menuItemId, int count) : base(id)
    {
        MenuItemId = menuItemId;
        Count = count;
    }

    public OrderLineItem(Guid id, MenuItem menuItem, int count) : base(id)
    {
        MenuItemId = menuItem.Id;
        MenuItem = menuItem;
        Count = count;
    }
}
