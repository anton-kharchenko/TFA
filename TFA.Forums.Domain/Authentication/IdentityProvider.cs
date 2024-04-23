using TFA.Forums.Domain.Interfaces.Authentication;

namespace TFA.Forums.Domain.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = default!;
}