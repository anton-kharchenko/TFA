namespace TFA.Forums.Domain.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken);
}