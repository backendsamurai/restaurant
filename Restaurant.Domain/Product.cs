namespace Restaurant.Domain
{
    public sealed class Product : Entity
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string ImageUrl { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;
        public ProductCategory Category { get; private set; } = default!;

        public Product() : base(Guid.NewGuid()) { }

        public Product(string name,
        string description,
        string imageUrl,
        decimal price,
        ProductCategory category) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Category = category;
        }

        public void ChangeName(string name) => Name = name;
        public void ChangeDescription(string description) => Description = description;
        public void ChangePrice(decimal price) => Price = price;
        public void ChangeCategory(ProductCategory category) => Category = category;
    }
}