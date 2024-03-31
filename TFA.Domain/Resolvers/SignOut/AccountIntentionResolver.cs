using TFA.Domain.Enums;
using TFA.Domain.Extensions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Resolvers.SignOut;

internal class AccountIntentionResolver : IIntentionResolver<AccountIntentionType>
{
    public bool IsAllowed(IIdentity identity, AccountIntentionType intention) =>
        intention switch
        {
            AccountIntentionType.SignOut => identity.IsAuthenticated(),
            _ => false
        };
}