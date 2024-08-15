namespace Restaurant.API.Entities;

public sealed class Payment : Entity
{
    public PaymentStatus Status { get; set; }
    public decimal Bill { get; set; }
    public decimal? Tip { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
