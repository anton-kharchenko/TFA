using TFA.Forum.Domain.Models;

namespace TFA.Forum.Domain.Interfaces.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<Topic> resources, int totalCount)> GetTopicsAsync(Guid forumId, int skip, int take,
        CancellationToken cancellationToken);
}