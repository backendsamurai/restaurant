namespace Restaurant.Shared.Models.Customer;

public sealed record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    bool IsVerified
);