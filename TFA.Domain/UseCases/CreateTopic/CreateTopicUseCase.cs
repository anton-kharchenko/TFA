using FluentValidation;
using MediatR;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Extensions.UseCases;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(
    IIntentionManager intentionManager,
    ICreateTopicStorage storage,
    IIdentityProvider identityProvider,
    IGetForumsStorage getForumsStorage) : IRequestHandler<CreateTopicCommand, Topic>
{
    public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        var (forumId, title) = (command.ForumId, command.Title);

        intentionManager.ThrowIfForbidden(TopicIntentionType.Create);

        await getForumsStorage.ThrowIfForumNotFoundAsync(forumId, cancellationToken);

        return await storage.CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}