namespace Restaurant.API.Dto.Responses;

public sealed class LoginCustomerResponse
{
    public Guid CustomerId { get; set; }
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string UserEmail { get; set; }
    public bool IsVerified { get; set; }
    public required string AccessToken { get; set; }
}
