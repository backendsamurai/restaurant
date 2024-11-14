using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Users
{
    public sealed class LikedProduct : Entity
    {
        // TODO: Product
        private LikedProduct() : base(Guid.NewGuid()) { }
    }
}