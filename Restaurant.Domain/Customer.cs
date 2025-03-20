namespace Restaurant.Domain
{
    public sealed class Customer : Entity
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        private Customer() : base(Guid.NewGuid()) { }

        public Customer(string name, string email, string passwordHash) : base(Guid.NewGuid())
        {
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void ChangeName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name != Name)
                Name = name;
        }

        public void ChangeEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email) && email != Email)
                Email = email;
        }

        public void ChangePasswordHash(string passwordHash)
        {
            if (!string.IsNullOrWhiteSpace(passwordHash))
                PasswordHash = passwordHash;
        }
    }
}