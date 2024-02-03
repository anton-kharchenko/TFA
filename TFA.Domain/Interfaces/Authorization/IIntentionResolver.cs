using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Interfaces.Authorization;

public interface IIntentionResolver;

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity identity, TIntention intention);
}