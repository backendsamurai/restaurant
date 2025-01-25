namespace Restaurant.Domain
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
    }
}