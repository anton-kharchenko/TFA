using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.GetForums;

public interface IGetForumsStorage
{
    Task<IEnumerable<Forum>> GetForumsAsync(CancellationToken cancellationToken);
}