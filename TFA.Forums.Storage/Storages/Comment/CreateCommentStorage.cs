using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Interfaces.Storages.Comment;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Interfaces;

namespace TFA.Forums.Storage.Storages.Comment;

internal class CreateCommentStorage(
    ForumDbContext forumDbContext, 
    IGuidFactory guidFactory,
    IMomentProvider momentProvider,
    IMapper mapper) : ICreateCommentStorage
{
    public async Task<Domain.Models.Comment> CreateAsync(Guid topicId, Guid userId, string text, CancellationToken cancellationToken)
    {
        var commentId = guidFactory.Create();
        await forumDbContext.Comments.AddAsync(new Entities.Comment
        {
            Id = commentId,
            TopicId = topicId,
            UserId = userId,
            CreatedAt = momentProvider.Now,
            Text = text
        }, cancellationToken);
        
        await forumDbContext.SaveChangesAsync(cancellationToken);

        return await forumDbContext.Comments
            .Where(c => c.Id == commentId)
            .ProjectTo<Domain.Models.Comment>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }

    public async Task<Domain.Models.Topic?> FindTopicAsync(Guid topicId, CancellationToken cancellationToken) =>
            await forumDbContext.Topics.Where(t=>t.TopicId == topicId)
                .ProjectTo<Domain.Models.Topic>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
}