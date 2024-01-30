using TFA.Storage;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> ExecuteAsync(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken);
}