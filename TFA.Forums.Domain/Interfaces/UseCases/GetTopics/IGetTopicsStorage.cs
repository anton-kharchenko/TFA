using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.Interfaces.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    Task<(IEnumerable<Topic> resources, int totalCount)> GetTopicsAsync(Guid forumId, int skip, int take,
        CancellationToken cancellationToken);
}