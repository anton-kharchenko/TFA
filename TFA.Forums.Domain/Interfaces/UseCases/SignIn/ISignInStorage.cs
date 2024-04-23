using TFA.Forums.Domain.Share;

namespace TFA.Forums.Domain.Interfaces.UseCases.SignIn;

public interface ISignInStorage
{
    Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken);

    Task<Guid> CreateSessionAsync(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
}