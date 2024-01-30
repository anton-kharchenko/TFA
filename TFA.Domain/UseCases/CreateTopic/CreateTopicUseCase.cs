using Microsoft.EntityFrameworkCore;
using TFA.Domain.Exceptions;
using TFA.Storage;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(IGuidFactory guidFactory, IMomentProvider momentProvider, ForumDbContext dbContext) : ICreateTopicUseCase
{
    public async Task<Models.Topic> ExecuteAsync(Guid forumId, string title, Guid authorId,
        CancellationToken cancellationToken)
    {
        var forumExists = await dbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        var topicId = guidFactory.Create();
        await dbContext.Topics.AddAsync(new Topic
        {
            TopicId = topicId,
            ForumId = forumId,
            UserId = authorId,
            Title = title,
            CreatedAt = momentProvider.Now
        }, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);


        return await dbContext.Topics.Where(t => t.TopicId == topicId)
            .Select(t => new Models.Topic
            {
                Id = t.TopicId,
                Title = t.Title,
                CreatedAt = t.CreatedAt,
                Author = t.Author.Login
            }).FirstAsync(cancellationToken);
    }
}