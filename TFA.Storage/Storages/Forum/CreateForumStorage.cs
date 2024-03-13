using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Storage.Helpers;

namespace TFA.Storage.Storages.Forum;

internal class CreateForumStorage(
    IGuidFactory guidFactory,
    ForumDbContext forumDbContext,
    IMemoryCache memoryCache) : ICreateForumStorage
{
    public async Task<Domain.Models.Forum> Create(string title, CancellationToken cancellationToken)
    {
        var forum = new Storage.Forum
        {
            ForumId = guidFactory.Create(),
            Title = title
        };

        await forumDbContext.Forums.AddAsync(forum, cancellationToken);
        await forumDbContext.SaveChangesAsync(cancellationToken);

        memoryCache.Remove(nameof(GetForumsStorage.GetForumsAsync));

        return await forumDbContext
            .Forums
            .Where(f => f.ForumId == forum.ForumId)
            .Select(f => new Domain.Models.Forum
            {
                Title = f.Title,
                Id = f.ForumId
            })
            .FirstAsync(cancellationToken);
    }
}