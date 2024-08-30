namespace Restaurant.API.Models.Customer;

public sealed record LoginCustomerResponse(
    Guid CustomerId,
    Guid UserId,
    string UserName,
    string UserEmail,
    bool IsVerified,
    string AccessToken
);
