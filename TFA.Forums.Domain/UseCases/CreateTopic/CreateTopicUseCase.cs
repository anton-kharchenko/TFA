using MediatR;
using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Extensions.UseCases;
using TFA.Forums.Domain.Interfaces.Storages.Forum;
using TFA.Forums.Domain.Commands.CreateTopic;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Events;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Domain.Interfaces.Storages.Topic;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    IGetForumsStorage getForumsStorage,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTopicCommand, Topic>
{
    public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var (forumId, title) = (command.ForumId, command.Title);

        intentionManager.ThrowIfForbidden(TopicIntentionType.Create);

        await getForumsStorage.ThrowIfForumNotFoundAsync(forumId, cancellationToken);

        await using var scope = await unitOfWork.StartScopeAsync(cancellationToken);

        var topic = await scope.GetStorage<ICreateTopicStorage>()
            .CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
        await scope.GetStorage<IDomainEventStorage>().AddEventAsync(ForumDomainEvent.TopicCreated(topic), cancellationToken);
        await scope.CommitAsync(cancellationToken);

        return topic;
    }
}