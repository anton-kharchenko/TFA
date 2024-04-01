using TFA.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Storage.Storages.SignOut;

public class SignOutStorage : ISignOutStorage
{
    public async Task RemoveSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}