using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Storage.Configurations;
using TFA.Storage.Helpers;
using TFA.Storage.Interfaces;

namespace TFA.Storage.Storages.Forum;

internal class CreateForumStorage(
    IGuidFactory guidFactory,
    ForumDbContext forumDbContext,
    IMemoryCache memoryCache,
    IMapper mapper) : ICreateForumStorage
{
    public async Task<Domain.Models.Forum> CreateAsync(string title, CancellationToken cancellationToken)
    {
        var forum = new Entities.Forum
        {
            ForumId = guidFactory.Create(),
            Title = title
        };

        await forumDbContext.Forums!.AddAsync(forum, cancellationToken);
        await forumDbContext.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(nameof(GetForumsStorage.GetForumsAsync));

        return await forumDbContext
            .Forums
            .Where(f => f.ForumId == forum.ForumId)
            .ProjectTo<Domain.Models.Forum>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}