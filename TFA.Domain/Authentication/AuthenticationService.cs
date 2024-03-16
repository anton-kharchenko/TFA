using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class AuthenticationService(IAuthenticationStorage storage) : IAuthenticationService
{
    public async Task<(bool success, string authToken)> SignInAsync(
        BasicSignInCredentials credentials,
        CancellationToken cancellationToken)
    {
        var recognisedUser = await storage.FindUserAsync(credentials.Login, cancellationToken);
        if (recognisedUser is null)
            throw new Exception("User not found");
        var success = credentials.Password + recognisedUser.Salt == recognisedUser.PasswordHash;
        return (success, credentials.Login);
    }

    public Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}