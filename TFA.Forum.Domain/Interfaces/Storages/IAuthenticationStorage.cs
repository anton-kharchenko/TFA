using Authentication_Session = TFA.Forum.Domain.Authentication.Session;
using Session = TFA.Forum.Domain.Authentication.Session;

namespace TFA.Forum.Domain.Interfaces.Storages;

public interface IAuthenticationStorage
{
    Task<Authentication_Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}