using System.Text;
using Restaurant.Services.Contracts;

namespace Restaurant.Services.Implementations;

public sealed class OtpGeneratorService : IOtpGeneratorService
{
    public string Generate(int length = 6)
    {
        StringBuilder _stringBuilder = new(0, length);

        for (int i = 0; i < length; i++)
        {
            _stringBuilder.Append(Random.Shared.Next(0, 9));
        }

        return _stringBuilder.ToString();
    }
}
