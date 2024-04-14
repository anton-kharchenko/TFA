using MediatR;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Extensions.UseCases;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

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

       var createTopicStorage = scope.GetStorage<ICreateTopicStorage>();
       var topic = await createTopicStorage.CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
       
       await scope.CommitAsync(cancellationToken);
       return topic;
    }
}