using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace AchievCor.Server.Data.Authorization;

public class Password
{
    private const int _saltSize = 16;
    private const int _hashSize = 32;
    private const int _iterations = 100000;

    public static string Hash(string password)
    {
        byte[] salt = new byte[_saltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        // Создаем хеш
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _iterations,
            numBytesRequested: _hashSize
        );

        byte[] hashBytes = new byte[_saltSize + _hashSize];
        Array.Copy(salt, 0, hashBytes, 0, _saltSize);
        Array.Copy(hash, 0, hashBytes, _saltSize, _hashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string password, string hashedPassword)
    {
        try
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[_saltSize];
            Array.Copy(hashBytes, 0, salt, 0, _saltSize);

            byte[] originalHash = new byte[_hashSize];
            Array.Copy(hashBytes, _saltSize, originalHash, 0, _hashSize);

            byte[] newHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterations,
                numBytesRequested: _hashSize
            );

            return CryptographicOperations.FixedTimeEquals(originalHash, newHash);
        }
        catch
        {
            return false;
        }
    }
}
