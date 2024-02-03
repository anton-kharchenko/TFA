using TFA.Domain.Interfaces;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; }
}