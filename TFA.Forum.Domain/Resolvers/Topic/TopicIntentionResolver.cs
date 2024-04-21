using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.UseCases.CreateTopic;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;

namespace TFA.Forum.Domain.Resolvers.Topic;

internal class TopicIntentionResolver : IIntentionResolver<TopicIntentionType>
{
    public bool IsAllowed(IIdentity identity, TopicIntentionType  intentionType)
    {
        return intentionType switch
        {
            TopicIntentionType.Create => identity.IsAuthenticated(),
            _ => false
        };
    }
}