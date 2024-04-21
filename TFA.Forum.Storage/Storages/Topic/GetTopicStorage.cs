using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forum.Storage.Configurations;

namespace TFA.Forum.Storage.Storages.Topic;

internal class GetTopicStorage(ForumDbContext dbContext) : IGetTopicsStorage
{
    public async Task<(IEnumerable<TFA.Forum.Domain.Models.Topic> resources, int totalCount)> GetTopicsAsync(Guid forumId,
        int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics!.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var resources = await query
            .Select(t => new TFA.Forum.Domain.Models.Topic
            {
                Title = t.Title,
                ForumId = t.ForumId,
                Id = t.ForumId,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId
            })
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}