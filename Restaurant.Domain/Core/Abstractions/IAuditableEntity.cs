namespace Restaurant.Domain.Core.Abstractions
{
    public interface IAuditableEntity
    {
        public DateTime CreatedOnUTC { get; }
        public DateTime? ModifiedOnUTC { get; }
    }
}