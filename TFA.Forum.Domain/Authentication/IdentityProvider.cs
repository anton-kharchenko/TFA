using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Forum.Domain.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = default!;
}