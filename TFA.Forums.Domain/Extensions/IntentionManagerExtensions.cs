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
    
    public static void ThrowIfForbidden<TIntention, TTarget>(this IIntentionManager intentionManager, 
        TIntention intention, TTarget target)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention, target)) throw new IntentionManagerException();
    }
}