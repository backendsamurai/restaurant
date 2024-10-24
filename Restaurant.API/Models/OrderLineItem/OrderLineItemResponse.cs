namespace Restaurant.API.Models.OrderLineItem;

public sealed record OrderLineItemResponse(string ProductName, decimal ProductPrice, int Count);