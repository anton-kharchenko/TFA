using TFA.Domain.Share;

namespace TFA.Storage.Interfaces;

public interface IAuthenticationStorage
{
    Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken);
    
}