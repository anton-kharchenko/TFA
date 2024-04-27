using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.Storage.Storages.Topic;

internal class GetTopicStorage(ForumDbContext dbContext, IMapper mapper) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Forums.Domain.Models.Topic> resources, int totalCount)> GetTopicsAsync(Guid forumId,
        int skip, int take, CancellationToken cancellationToken)
    {
        var query = dbContext.Topics!.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var topicListItemReadModels = await dbContext.Database.SqlQuery<TopicListItemReadModel>($@"
SELECT t.""TopicId""                       as ""Id"",
       t.""ForumId""                       as  ""ForumId"",
       t.""UserId""                        as  ""UserId"",
       t.""Title""                         as ""Title"",
       t.""CreateAt""                      as ""CreateAt"",
       coalesce(t.TotalCommentsCount, 0) as TotalCommentsCount,
       c.""CreateAt"" as ""LastCommentCreateAt"",
       c.""CommentId"" as ""LastCommentId""
FROM ""Topics"" t
         LEFT JOIN (SELECT ""TopicId"",
                           ""CommentId"",
                           ""CreateAt"",
                           Count(*) OVER (PARTITION BY ""TopicId"") as                            TotalCommentsCount,
                           ROW_NUMBER() OVER (PARTITION BY ""TopicId"" ORDER BY ""CreateAt"" DESC ) rn
                    FROM ""Comments"") as c on t.""TopicId"" = c.""TopicId"" and c.rn = 1
WHERE t.""ForumId"" = {forumId}
ORDER BY COALESCE(c.""CreateAt"", t.""CreateAt"") DESC
LIMIT {take} offset {skip}")
            .ProjectTo<Entities.Topic>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

        var resources = await query
            .ProjectTo<Forums.Domain.Models.Topic>(mapper.ConfigurationProvider)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}