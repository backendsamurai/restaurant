using System.Text;
using Restaurant.API.Security.Services.Contracts;

namespace Restaurant.API.Security.Services;

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
