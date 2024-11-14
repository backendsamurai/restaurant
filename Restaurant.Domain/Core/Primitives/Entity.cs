using Restaurant.Domain.Core.Guards;

namespace Restaurant.Domain.Core.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; private set; }

        protected Entity() { }

        protected Entity(Guid id)
        {
            Ensure.NotEmpty(id, "The identifier is required", nameof(id));
            Id = id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        public bool Equals(Entity? other)
        {
            if (other is null)
                return false;

            return ReferenceEquals(this, other) || Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            if (obj is not Entity entity)
                return false;

            if (Id == Guid.Empty || entity.Id == Guid.Empty)
                return false;

            return Id == entity.Id;
        }

        public override int GetHashCode() => Id.GetHashCode() * 41;
    }
}