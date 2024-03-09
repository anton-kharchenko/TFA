namespace TFA.Domain.Interfaces.Storages.Topic;

public interface ICreateTopicStorage
{
    Task<Models.Topic> CreateTopicAsync(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
}