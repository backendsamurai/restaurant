using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Products
{
    public sealed class Product : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public decimal OldPrice { get; private set; }
        public decimal Price { get; private set; }
        public ProductCategory Category { get; private set; }

        private Product
        (
            string name,
            string description,
            string imageUrl,
            decimal oldPrice,
            decimal price,
            ProductCategory productCategory
        ) : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            OldPrice = oldPrice;
            Price = price;
            Category = productCategory;
        }
    }
}