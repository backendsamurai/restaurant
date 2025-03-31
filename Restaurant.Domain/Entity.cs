namespace Restaurant.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        protected Entity(Guid id) : this()
        {
            Id = id;
        }

        // Required by EF Core
        protected Entity() { }
    }
}