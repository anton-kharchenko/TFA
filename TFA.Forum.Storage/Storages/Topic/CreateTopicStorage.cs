using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Interfaces.Storages.Topic;
using TFA.Forum.Storage.Configurations;
using TFA.Forum.Storage.Interfaces;
using TFA.Forum.Storage.Helpers;

namespace TFA.Forum.Storage.Storages.Topic;

internal class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    ForumDbContext dbContext) : ICreateTopicStorage
{
    public async Task<TFA.Forum.Domain.Models.Topic> CreateTopicAsync(
        Guid forumId,
        Guid userId,
        string title,
        CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Entities.Topic
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = userId,
            Title = title,
            CreatedAt = momentProvider.Now
        };

        await dbContext.Topics!.AddAsync(topic, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Topics
            .Where(t => t.TopicId == topicId)
            .Select(t => new TFA.Forum.Domain.Models.Topic
            {
                Id = topicId,
                ForumId = forumId,
                UserId = userId,
                Title = title,
                CreatedAt = momentProvider.Now
            }).FirstAsync(cancellationToken);
    }
}