namespace Restaurant.Domain
{
    public sealed class ProductCategory(string name) : Entity(Guid.NewGuid())
    {
        public string Name { get; private set; } = name;

        public void ChangeName(string name) => Name = name;
    }
}