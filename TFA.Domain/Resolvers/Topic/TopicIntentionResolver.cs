using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.UseCases.CreateTopic;

namespace TFA.Domain.Resolvers.Topic;

internal class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity identity, TopicIntention  intention)
    {
        return intention switch
        {
            TopicIntention.Create => identity.IsAuthenticated(),
            _ => false
        };
    }
}