using TFA.Forums.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Forums.Storage.Storages.SignOut;

public class SignOutStorage : ISignOutStorage
{
    public Task RemoveSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}