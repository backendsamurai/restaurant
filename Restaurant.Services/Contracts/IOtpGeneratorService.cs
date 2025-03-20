namespace Restaurant.Services.Contracts;

public interface IOtpGeneratorService
{
    public string Generate(int length = 6);
}
