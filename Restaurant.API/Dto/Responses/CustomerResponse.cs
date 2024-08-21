namespace Restaurant.API.Dto.Responses;

public sealed class CustomerResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsVerified { get; set; }
}
