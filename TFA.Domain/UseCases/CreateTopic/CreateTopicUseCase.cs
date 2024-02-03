using TFA.Domain.Exceptions;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.UseCases.CreateTopic;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(IIntentionManager intentionManager, ICreateTopicStorage storage, IIdentityProvider identityProvider) : ICreateTopicUseCase
{
    public async Task<Models.Topic> ExecuteAsync(Guid forumId, string title, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        var forumExists = await storage.ForumExistsAsync(forumId, cancellationToken);
        
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        return await storage.CreateTopicAsync(forumId, identityProvider.Current.UserId, title, cancellationToken);
    }
}