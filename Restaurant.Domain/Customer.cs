namespace Restaurant.Domain
{
    public sealed class Customer : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public Customer(string name, string email, string passwordHash) : base(Guid.NewGuid())
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}