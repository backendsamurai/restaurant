namespace Restaurant.API.Dto.Requests;

public sealed class UpdateCustomerRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}