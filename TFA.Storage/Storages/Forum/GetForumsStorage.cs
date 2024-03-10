using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.Interfaces.UseCases.GetForums;

namespace TFA.Storage.Storages;

internal class GetForumsStorage(ForumDbContext dbContext, IMemoryCache memoryCache) : IGetForumsStorage
{
    public async Task<IEnumerable<Domain.Models.Forum>> GetForumsAsync(CancellationToken cancellationToken)
    {
        return (await memoryCache.GetOrCreateAsync(nameof(GetForumsAsync), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return dbContext.Forums.Select(i => new Domain.Models.Forum
            {
                Id = i.ForumId,
                Title = i.Title
            }).ToArrayAsync(cancellationToken);

        }))!;
    }
}