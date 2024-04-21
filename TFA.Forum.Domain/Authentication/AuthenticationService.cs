using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TFA.Forum.Domain.Configurations;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Storages;

namespace TFA.Forum.Domain.Authentication;

internal class AuthenticationService(
    ISymmetricDecryptor symmetricDecryptor,
    IOptions<AuthenticationConfiguration> authenticationConfiguration,
    IAuthenticationStorage authenticationStorage,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private AuthenticationConfiguration? configuration;


    public async Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken)
    {
        string sessionIdStr;

        try
        {
            configuration = authenticationConfiguration.Value;
            sessionIdStr = await symmetricDecryptor.DecryptAsync(authToken, configuration.Key, cancellationToken);
        }
        catch (CryptographicException)
        {
            logger.LogWarning("Cannot decrypt auth token");
            return User.Guest;
        }

        if (!Guid.TryParse(sessionIdStr, out var sessionId))
        {
            return User.Guest;
        }

        var session = await authenticationStorage.FindSessionAsync(sessionId, cancellationToken);
        
        if (session is null)
        {
            return User.Guest;
        }

        if (session.ExpireAt < DateTimeOffset.Now)
        {
            return User.Guest;
        }
        
        return new User(session!.UserId, sessionId);
    }
}