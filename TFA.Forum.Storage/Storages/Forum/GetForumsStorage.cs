using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Storage.Configurations;

namespace TFA.Forum.Storage.Storages.Forum;

internal class GetForumsStorage(
    ForumDbContext dbContext,
    IMemoryCache memoryCache,
    IMapper mapper) : IGetForumsStorage
{
    public async Task<IEnumerable<TFA.Forum.Domain.Models.Forum>> GetForumsAsync(CancellationToken cancellationToken)
    {
        return (await memoryCache.GetOrCreateAsync(nameof(GetForumsAsync), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return dbContext
                .Forums
                .ProjectTo<TFA.Forum.Domain.Models.Forum>(mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
        }))!;
    }
}