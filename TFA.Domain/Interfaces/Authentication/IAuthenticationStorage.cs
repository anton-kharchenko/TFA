using TFA.Domain.Share;

namespace TFA.Domain.Interfaces.Authentication;

public interface IAuthenticationStorage
{
    Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken);
}