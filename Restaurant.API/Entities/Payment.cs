using Restaurant.API.Entities.Abstractions;

namespace Restaurant.API.Entities;

public sealed class Payment : AuditableEntity
{
    public decimal Bill { get; set; }
    public decimal? Tip { get; set; }
}
