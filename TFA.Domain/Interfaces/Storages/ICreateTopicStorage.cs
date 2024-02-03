using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.Storages;

public interface ICreateTopicStorage
{
    Task<bool> ForumExistsAsync(Guid forumId, CancellationToken cancellationToken);

    Task<Topic> CreateTopicAsync(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
}