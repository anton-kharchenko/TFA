using System.Security.Cryptography;
using System.Text;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class SecurityManager : ISecurityManager
{
    const int saltLength = 100;
    
    private readonly Lazy<SHA256> _sha256 = new(SHA256.Create);
    public bool ComparePasswords(string password, string salt, string hash)
    {
        var newHash =  ComputeSHA(password, salt);

        return string.Equals(Encoding.UTF8.GetString(newHash), Encoding.UTF8.GetString(Convert.FromBase64String(hash)));
    }

    public (string salt, string hash) GeneratePasswordParts(string password)
    {
        var buffer = RandomNumberGenerator.GetBytes(saltLength * 4 / 3);
        var base64String = Convert.ToBase64String(buffer);
        var salt = base64String.Length > saltLength ? base64String[..saltLength] : base64String;
        
        var hash = ComputeSHA(password, salt);
        
        return (salt, Convert.ToBase64String(hash));
    }

    private byte[] ComputeSHA(string plainText, string salt)
    {
        var buffer = Encoding.UTF8.GetBytes(plainText).Concat(Convert.FromBase64String(salt)).ToArray();
        
        lock (_sha256) 
            return _sha256.Value.ComputeHash(buffer);
    }
    
    
}