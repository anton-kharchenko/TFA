namespace TFA.Forums.Domain.Interfaces.UseCases.SignOn;

public interface ISignOnStorage
{
    Task<Guid> CreateUserAsync(string login, byte[] salt, byte[] hash, CancellationToken token);
}