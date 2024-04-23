using TFA.Forums.Domain.Exceptions;
using TFA.Forums.Domain.Interfaces.Authorization;

namespace TFA.Forums.Domain.Extensions;

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention)) throw new IntentionManagerException();
    }
}