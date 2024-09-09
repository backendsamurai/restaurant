namespace Restaurant.API.Models.Customer;

public sealed record CustomerResponse(
    Guid CustomerId,
    Guid UserId,
    string UserName,
    string UserEmail,
    bool IsVerified
);