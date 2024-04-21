using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Forum.Domain.Interfaces.Authorization;

internal interface IIntentionResolver;

internal interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity identity, TIntention intention);
}