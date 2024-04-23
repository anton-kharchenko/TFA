using TFA.Forums.Domain.Extensions;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;

namespace TFA.Forums.Domain.Resolvers.SignOut;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntentionType>
{
    public bool IsAllowed(IIdentity identity, AccountIntentionType intention) =>
        intention switch
        {
            AccountIntentionType.SignOut => identity.IsAuthenticated(),
            _ => false
        };
}