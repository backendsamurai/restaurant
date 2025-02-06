namespace Restaurant.Domain
{
    public sealed class Customer(string name, string email, string passwordHash) : Entity(Guid.NewGuid())
    {
        public string Name { get; private set; } = name;
        public string Email { get; private set; } = email;
        public string PasswordHash { get; private set; } = passwordHash;
    }
}