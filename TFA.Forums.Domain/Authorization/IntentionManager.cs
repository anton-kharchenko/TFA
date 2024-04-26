using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Interfaces.Authorization;

namespace TFA.Forums.Domain.Authorization;

internal class IntentionManager(IEnumerable<IIntentionResolver> resolvers, IIdentityProvider identityProvider)
    : IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var resolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return resolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }

    public bool IsAllowed<TIntention, TTarget>(TIntention intention, TTarget target) where TIntention : struct
    {
        var resolver = resolvers.OfType<IIntentionResolver<TIntention, TTarget>>().FirstOrDefault();
        return resolver?.IsAllowed(identityProvider.Current, intention, target) ?? false;
    }
}