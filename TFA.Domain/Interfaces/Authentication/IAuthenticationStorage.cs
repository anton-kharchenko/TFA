using TFA.Domain.Authentication;

namespace TFA.Domain.Interfaces.Authentication;

public interface IAuthenticationStorage
{
    Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken);
}