using Restaurant.Domain;

namespace Restaurant.API.Security.Models;

public sealed class AuthenticatedUser
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsVerified { get; set; }
    public UserRole UserRole { get; set; }
}
