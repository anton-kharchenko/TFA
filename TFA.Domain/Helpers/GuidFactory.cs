using TFA.Domain.Interfaces.Helpers;

namespace TFA.Domain.Helpers;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}