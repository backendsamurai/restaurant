using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Products
{
    public sealed class ProductCategory : Entity
    {
        public string Name { get; private set; }
        public DiscountRange DiscountRange { get; private set; }

        private ProductCategory(string name, DiscountRange discountRange) : base(Guid.NewGuid())
        {
            Name = name;
            DiscountRange = discountRange;
        }

        public static ProductCategory Create(string name, DiscountRange discountRange) => new(name, discountRange);
    }
}