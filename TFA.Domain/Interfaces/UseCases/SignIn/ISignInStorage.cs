using TFA.Domain.Share;

namespace TFA.Domain.Interfaces.UseCases.SignIn;

public interface ISignInStorage
{
    Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken);

    Task<Guid> CreateSessionAsync(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
}