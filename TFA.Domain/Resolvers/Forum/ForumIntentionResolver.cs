using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Resolvers.Forum;

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