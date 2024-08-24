namespace Restaurant.API.Dto.Requests;

public sealed class LoginUserRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
