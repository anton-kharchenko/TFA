using TFA.Forums.Domain.Interfaces.Authentication;

namespace TFA.Forums.Domain.Interfaces.Authorization;

internal interface IIntentionResolver;

internal interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity identity, TIntention intention);
}

internal interface IIntentionResolver<in TIntention, in TTarget> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention, TTarget target);
}