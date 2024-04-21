using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;

namespace TFA.Forum.Domain.Resolvers.Forum;

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