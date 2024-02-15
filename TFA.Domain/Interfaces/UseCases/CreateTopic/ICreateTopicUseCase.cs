using TFA.Domain.Validations.CreateTopic;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Interfaces.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    Task<Topic> ExecuteAsync(CreateTopicCommand createTopicCommand, CancellationToken cancellationToken);
}