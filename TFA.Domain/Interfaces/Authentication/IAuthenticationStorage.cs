using TFA.Domain.Authentication;

namespace TFA.Domain.Interfaces.Authentication;

public interface IAuthenticationStorage
{
    Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}