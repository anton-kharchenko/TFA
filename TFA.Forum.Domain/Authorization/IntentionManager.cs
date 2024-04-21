using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;

namespace TFA.Forum.Domain.Authorization;

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
        throw new NotImplementedException();
    }
}