using TFA.Storage.Interfaces;

namespace TFA.Storage.Helpers;

public class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}