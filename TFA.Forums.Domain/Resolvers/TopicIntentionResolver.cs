using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;
using TFA.Forums.Domain.Models;

namespace TFA.Forums.Domain.Resolvers;

internal class TopicIntentionResolver : 
    IIntentionResolver<TopicIntentionType>,
    IIntentionResolver<TopicIntentionType, Topic>
{
    public bool IsAllowed(IIdentity identity, TopicIntentionType  intention) =>
        intention switch
        {
            TopicIntentionType.Create => identity.IsAuthenticated(),
            _ => false
        };

    public bool IsAllowed(IIdentity subject, TopicIntentionType intention, Topic target)  =>
        intention switch
    {
        TopicIntentionType.CreateComment => subject.IsAuthenticated(),
        _ => false
    };
}