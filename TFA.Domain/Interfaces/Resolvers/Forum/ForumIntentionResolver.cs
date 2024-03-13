using TFA.Domain.Enums.Forum;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Interfaces.Resolvers.Forum;

internal class ForumIntentionResolver : IIntentionResolver<ForumIntention>
{
    public bool IsAllowed(IIdentity identity, ForumIntention intention)
    {
        return intention switch
        {
            ForumIntention.Create => identity.IsAuthenticated(),
            _ => false
        };
    }
}