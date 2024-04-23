using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Storage.Configurations;

namespace TFA.Forums.Storage.Storages.Forum;

internal class GetForumsStorage(
    ForumDbContext dbContext,
    IMemoryCache memoryCache,
    IMapper mapper) : IGetForumsStorage
{
    public async Task<IEnumerable<Forums.Domain.Models.Forum>> GetForumsAsync(CancellationToken cancellationToken)
    {
        return (await memoryCache.GetOrCreateAsync(nameof(GetForumsAsync), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return dbContext
                .Forums
                .ProjectTo<Forums.Domain.Models.Forum>(mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
        }))!;
    }
}