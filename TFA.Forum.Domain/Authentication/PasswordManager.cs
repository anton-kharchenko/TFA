using System.Security.Cryptography;
using System.Text;
using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Forum.Domain.Authentication;

internal class PasswordManager : IPasswordManager
{
    private const int SaltLength = 100;

    private readonly Lazy<SHA256> _sha256 = new(SHA256.Create);

    public bool ComparePasswords(string password, byte[] salt, byte[] hash) =>
        ComputeHash(password, salt).SequenceEqual(hash);

    public (byte[] salt, byte[] hash) GeneratePasswordParts(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);

        var hash = ComputeHash(password, salt);

        return (salt, hash.ToArray());
    }

    private ReadOnlySpan<byte> ComputeHash(string text, byte[] salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(text);

        var buffer = new byte[plainTextBytes.Length + salt.Length];

        Array.Copy(plainTextBytes, buffer, plainTextBytes.Length);

        Array.Copy(salt, 0, buffer, plainTextBytes.Length, salt.Length);

        lock (_sha256)
        {
            return _sha256.Value.ComputeHash(buffer);
        }
    }
}