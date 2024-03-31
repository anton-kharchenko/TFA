using TFA.Domain.Enums;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Interfaces.Authorization;

internal interface IIntentionResolver;

internal interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity identity, TIntention intention);
}