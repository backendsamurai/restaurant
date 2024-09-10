namespace Restaurant.API.Security.Services.Contracts;

public interface IOtpGeneratorService
{
    public string Generate(int length = 6);
}
