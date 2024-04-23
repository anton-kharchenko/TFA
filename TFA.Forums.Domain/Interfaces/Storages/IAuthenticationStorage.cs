using Authentication_Session = TFA.Forums.Domain.Authentication.Session;
using Domain_Authentication_Session = TFA.Forums.Domain.Authentication.Session;
using Session = TFA.Forums.Domain.Authentication.Session;

namespace TFA.Forums.Domain.Interfaces.Storages;

public interface IAuthenticationStorage
{
    Task<Domain_Authentication_Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}