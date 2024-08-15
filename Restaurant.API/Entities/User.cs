namespace Restaurant.API.Entities;

public sealed class User : Entity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public bool IsVerified { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
}
