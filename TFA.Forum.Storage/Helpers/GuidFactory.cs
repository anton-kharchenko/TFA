using TFA.Forum.Storage.Interfaces;

namespace TFA.Forum.Storage.Helpers;

public class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}