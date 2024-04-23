namespace TFA.Forums.Domain.Interfaces.UseCases.SignOut;

public interface ISignOutStorage
{
    Task RemoveSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}