using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Interfaces.Storages.Topic;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Interfaces;

namespace TFA.Forums.Storage.Storages.Topic;

internal class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    IMapper mapper,
    ForumDbContext dbContext) : ICreateTopicStorage
{
    public async Task<Forums.Domain.Models.Topic> CreateTopicAsync(
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
            .ProjectTo<Forums.Domain.Models.Topic>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}