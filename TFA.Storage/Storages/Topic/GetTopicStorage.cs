using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Storage.Configurations;

namespace TFA.Storage.Storages.Topic;

internal class GetTopicStorage(ForumDbContext dbContext) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Domain.Models.Topic> resources, int totalCount)> GetTopicsAsync(Guid forumId,
        int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics!.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var resources = await query
            .Select(t => new Domain.Models.Topic
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