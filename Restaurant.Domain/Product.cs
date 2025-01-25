namespace Restaurant.Domain
{
    public sealed class Product : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public ProductCategory Category { get; private set; }

        public Product(
            string name,
            string description,
            string imageUrl,
            decimal price,
            ProductCategory category
        ) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Category = category;
        }
    }
}