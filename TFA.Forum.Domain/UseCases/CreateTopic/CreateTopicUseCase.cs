using MediatR;
using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Extensions.UseCases;
using TFA.Forum.Domain.Interfaces.Storages.Forum;
using TFA.Forum.Domain.Commands.CreateTopic;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;
using TFA.Forum.Domain.Interfaces.Storages;
using TFA.Forum.Domain.Interfaces.Storages.Topic;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Models;

namespace TFA.Forum.Domain.UseCases.CreateTopic;

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