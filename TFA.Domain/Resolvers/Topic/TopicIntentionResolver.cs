using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Keys;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Resolvers.Topic;

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