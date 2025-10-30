using System.Security.Cryptography;

namespace AchievCor.Server.Data.Authorization;

public class JwtSecretGenerator
{
    public static string Generate(int byteLength = 48)
    {
       var bytes = new byte[byteLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
