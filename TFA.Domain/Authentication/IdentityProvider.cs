using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = default!;
}