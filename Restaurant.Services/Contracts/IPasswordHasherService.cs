namespace Restaurant.Services.Contracts;

public interface IPasswordHasherService
{
    public string Hash(string password);

    public bool Verify(string password, string passwordHash);
}
