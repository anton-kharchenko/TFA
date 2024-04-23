using TFA.Forums.Storage.Interfaces;

namespace TFA.Forums.Storage.Helpers;

public class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}