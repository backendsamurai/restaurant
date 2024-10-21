namespace Restaurant.API.Models.Payment;

public sealed record CreatePaymentModel(decimal Bill, decimal? Tip);