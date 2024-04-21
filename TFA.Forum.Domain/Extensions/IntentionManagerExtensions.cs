using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.Interfaces.Authorization;

namespace TFA.Forum.Domain.Extensions;

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention)) throw new IntentionManagerException();
    }
}