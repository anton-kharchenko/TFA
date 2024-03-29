﻿using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Storage.Configurations;
using TFA.Storage.Helpers;

namespace TFA.Storage.Storages.Topic;

internal class CreateTopicStorage(
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    ForumDbContext dbContext) : ICreateTopicStorage
{
    public Task<bool> ForumExistsAsync(Guid forumId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Domain.Models.Topic> CreateTopicAsync(
        Guid forumId,
        Guid userId,
        string title,
        CancellationToken cancellationToken)
    {
        var topicId = guidFactory.Create();
        var topic = new Models.Topic
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