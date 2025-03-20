namespace Restaurant.Shared.Models.Customer;

public sealed record CreateCustomerModel(string? Name, string? Email, string? Password);