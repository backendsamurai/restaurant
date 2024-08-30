namespace Restaurant.API.Models.Customer;

public sealed record CustomerResponse(
    Guid Id,
    Guid UserId,
    string Name,
    string Email,
    bool IsVerified
);