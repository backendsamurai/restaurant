namespace Restaurant.Shared.Models.OrderLineItem;

public sealed record OrderLineItemResponse(string ProductName, decimal ProductPrice, int Count);