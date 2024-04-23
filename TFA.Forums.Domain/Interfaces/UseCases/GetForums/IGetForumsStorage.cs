using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Interfaces.Storages;

namespace TFA.Forums.Domain.Interfaces.UseCases.GetForums;

public interface IGetForumsStorage : IStorage
{
    Task<IEnumerable<Models.Forum>> GetForumsAsync(CancellationToken cancellationToken);
}