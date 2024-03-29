using Microsoft.Extensions.Options;
using TFA.Domain.Configurations;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class AuthenticationService(
    ISymmetricDecryptor symmetricDecryptor,
    IOptions<AuthenticationConfiguration> authenticationConfiguration) : IAuthenticationService
{
    private AuthenticationConfiguration? configuration;


    public async  Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken)
    {
        configuration = authenticationConfiguration.Value;

        var userId = await symmetricDecryptor.DecryptAsync(authToken, configuration.Key, cancellationToken);

        // TODO: verify user id
        return new User(Guid.Parse(userId), Guid.Empty);
    }
}