using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class AuthenticationService(
    IAuthenticationStorage storage, 
    IPasswordManager passwordManager,
    Lazy<Aes> aes,
    IOptions<AuthenticationConfiguration> authenticationConfiguration) : IAuthenticationService
{
    public async Task<(bool success, string authToken)> SignInAsync(
        BasicSignInCredentials credentials,
        CancellationToken cancellationToken)
    {
        var recognisedUser = await storage.FindUserAsync(credentials.Login, cancellationToken);
        if (recognisedUser is null)
            throw new Exception("User not found");
            
        passwordManager.ComparePasswords(credentials.Password, recognisedUser.Salt, recognisedUser.PasswordHash);  
          
        var success = credentials.Password + recognisedUser.Salt == recognisedUser.PasswordHash;
        var userIdBytes = recognisedUser.UserId.ToByteArray();

        using var encryptedStream = new MemoryStream();
        
        var key = Convert.FromBase64String(authenticationConfiguration.Value.Key);
        var iv = Convert.FromBase64String(authenticationConfiguration.Value.Iv);
        
        await using (var stream = new CryptoStream(encryptedStream, aes.Value.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            await stream.WriteAsync(userIdBytes, cancellationToken);
        }
        
        
        return (success, Convert.ToBase64String(encryptedStream.ToArray()));
    }

    public async Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken)
    {
        using var decryptedSteam = new MemoryStream();
        
        var key = Convert.FromBase64String(authenticationConfiguration.Value.Key);
        var iv = Convert.FromBase64String(authenticationConfiguration.Value.Iv);
        
        await using (var stream = new CryptoStream(decryptedSteam, aes.Value.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            var encryptedBytes = Convert.FromBase64String(authToken);
            await stream.WriteAsync(encryptedBytes, cancellationToken);
        }

        var userId = new Guid(decryptedSteam.ToArray());
        
        return new User(userId);
    }
}