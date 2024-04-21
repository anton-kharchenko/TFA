using TFA.Forum.Domain.Extensions;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;

namespace TFA.Forum.Domain.Resolvers.SignOut;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntentionType>
{
    public bool IsAllowed(IIdentity identity, AccountIntentionType intention) =>
        intention switch
        {
            AccountIntentionType.SignOut => identity.IsAuthenticated(),
            _ => false
        };
}