namespace Restaurant.Shared.Models.Customer;

public sealed record UpdateCustomerModel(string? Name, string? Email, string? Password);