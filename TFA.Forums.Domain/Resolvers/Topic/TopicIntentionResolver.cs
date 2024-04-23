using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Keys;
using TFA.Forums.Domain.UseCases.CreateTopic;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;

namespace TFA.Forums.Domain.Resolvers.Topic;

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