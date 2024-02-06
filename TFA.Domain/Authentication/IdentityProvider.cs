using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => new User(Guid.Parse("e2a7d6f0-fcbd-40d7-aa1e-0080a8852f6a"));
}