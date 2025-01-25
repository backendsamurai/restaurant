namespace Restaurant.Domain
{
    public sealed class ProductCategory : Entity
    {
        public string Name { get; private set; }

        public ProductCategory(string name) : base(Guid.NewGuid())
        {
            Name = name;
        }
    }
}