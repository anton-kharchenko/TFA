namespace TFA.Domain.Interfaces.Storages.Topic;

public interface ICreateTopicStorage
{
    Task<bool> ForumExistsAsync(Guid forumId, CancellationToken cancellationToken);

    Task<Models.Topic> CreateTopicAsync(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
}