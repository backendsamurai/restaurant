namespace Restaurant.Shared.Models.Payment;

public sealed record CreatePaymentModel(Guid OrderId, decimal Bill, decimal? Tip);