using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

public interface ICreateTokenStorage
{
    Task<bool> ForumExistsAsync(Guid forumId, CancellationToken cancellationToken);

    Task<Topic> CreateTopicAsync(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
}