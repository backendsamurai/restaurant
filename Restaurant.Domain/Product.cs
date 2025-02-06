namespace Restaurant.Domain
{
    public sealed class Product(
        string name,
        string description,
        string imageUrl,
        decimal price,
        ProductCategory category
        ) : Entity(Guid.NewGuid())
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string ImageUrl { get; private set; } = imageUrl;
        public decimal Price { get; private set; } = price;
        public ProductCategory Category { get; private set; } = category;
    }
}