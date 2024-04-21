using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Interfaces.Storages;

namespace TFA.Forum.Domain.Interfaces.UseCases.GetForums;

public interface IGetForumsStorage : IStorage
{
    Task<IEnumerable<Models.Forum>> GetForumsAsync(CancellationToken cancellationToken);
}