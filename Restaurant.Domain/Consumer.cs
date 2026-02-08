namespace Restaurant.Domain;

public sealed class Consumer : Entity<Guid>, IAuditable, ISoftDeletable
{
    public string Name { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTimeOffset? DeletedAtUtc { get; private set; }

    public Consumer(
        Guid id, string name,
        string email, string passwordHash) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));

        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    public void ChangeName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;
    }

    public void ChangeEmail(string? email)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
    }

    public void ChangePasswordHash(string? passwordHash)
    {
        if (!string.IsNullOrWhiteSpace(passwordHash))
            PasswordHash = passwordHash;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTimeOffset.UtcNow;
    }
}
