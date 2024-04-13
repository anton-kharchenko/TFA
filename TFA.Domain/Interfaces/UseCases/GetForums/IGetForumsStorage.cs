using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.GetForums;

public interface IGetForumsStorage : IStorage
{
    Task<IEnumerable<Forum>> GetForumsAsync(CancellationToken cancellationToken);
}