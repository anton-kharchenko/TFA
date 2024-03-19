using TFA.Domain.Authentication;

namespace TFA.Domain.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken);
}