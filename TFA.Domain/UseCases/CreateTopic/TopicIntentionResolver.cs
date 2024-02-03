using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.UseCases.CreateTopic;

public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity identity, TopicIntention intention)
    {
        return intention switch
        {
            TopicIntention.Create => identity.IsAuthenticated(),
            _ => false
        };
    }
}