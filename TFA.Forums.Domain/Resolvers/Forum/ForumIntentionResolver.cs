using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;

namespace TFA.Forums.Domain.Resolvers.Forum;

internal class ForumIntentionResolver : IIntentionResolver<ForumIntentionType>
{
    public bool IsAllowed(IIdentity identity, ForumIntentionType intention)
    {
        return intention switch
        {
            ForumIntentionType.Create => identity.IsAuthenticated(),
            _ => false
        };
    }
}