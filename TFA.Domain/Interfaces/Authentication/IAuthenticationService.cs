using TFA.Domain.Authentication;

namespace TFA.Domain.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<(bool success, string authToken)> SignInAsync(BasicSignInCredentials credentials, CancellationToken cancellationToken);
    
    Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken);
}