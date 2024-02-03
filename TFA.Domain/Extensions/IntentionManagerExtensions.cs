using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Extensions;

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention))
        {
            throw new IntentionManagerException();
        }
    }
}