using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.Helpers;
using TFA.Domain.Interfaces.Storages;

namespace TFA.Storage.Storages;

public class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    ForumDbContext dbContext) : ICreateTopicStorage
{
    public async Task<bool> ForumExistsAsync(Guid forumId, CancellationToken cancellationToken) =>
        await dbContext.Forums.AnyAsync(i => i.ForumId == forumId, cancellationToken: cancellationToken);

    public async Task<Domain.Models.Topic> CreateTopicAsync(Guid forumId, Guid userId, string title, CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Topic
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = userId,
            Title = title,
            CreatedAt = momentProvider.Now
        };

        await dbContext.Topics.AddAsync(topic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new Domain.Models.Topic
            {
                Id = topicId,
                ForumId = forumId,
                UserId = userId,
                Title = title,
                CreatedAt = momentProvider.Now
            }).FirstAsync(cancellationToken);
    }
}