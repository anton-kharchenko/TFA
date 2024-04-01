using Session = TFA.Domain.Authentication.Session;

namespace TFA.Domain.Interfaces.Storages;

public interface IAuthenticationStorage
{
    Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}