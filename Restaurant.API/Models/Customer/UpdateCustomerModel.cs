namespace Restaurant.API.Models.Customer;

public sealed record UpdateCustomerModel(string? Name, string? Email, string? Password);