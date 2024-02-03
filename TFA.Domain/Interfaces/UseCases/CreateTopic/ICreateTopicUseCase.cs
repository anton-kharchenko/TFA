using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Interfaces.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> ExecuteAsync(Guid forumId, string title, CancellationToken cancellationToken);
}