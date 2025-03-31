namespace Restaurant.Domain
{
    public sealed class ProductCategory : Entity
    {
        public string Name { get; private set; } = default!;

        private ProductCategory() { }

        private ProductCategory(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public static ProductCategory Create(Guid id, string name) => new(id, name);

        public void ChangeName(string name) => Name = name;
    }
}