using TFA.Forum.Storage.Interfaces;

namespace TFA.Forum.Storage.Helpers;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}