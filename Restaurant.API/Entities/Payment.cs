namespace Restaurant.API.Entities;

public sealed class Payment : Entity
{
    public required Order Order { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Bill { get; set; }
    public decimal Tip { get; set; }
}
