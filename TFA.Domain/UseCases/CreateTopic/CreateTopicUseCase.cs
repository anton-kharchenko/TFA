using FluentValidation;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Extensions;
using TFA.Domain.Extensions.UseCases;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.CreateTopic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(
    IIntentionManager intentionManager,
    ICreateTopicStorage storage,
    IIdentityProvider identityProvider,
    IGetForumsStorage getForumsStorage,
    IValidator<CreateTopicCommand> validator) : ICreateTopicUseCase
{
    public async Task<Topic> ExecuteAsync(CreateTopicCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (forumId, title) = (command.ForumId, command.Title);

        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        await getForumsStorage.ThrowIfForumNotFoundAsync(forumId, cancellationToken);

        return await storage.CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}