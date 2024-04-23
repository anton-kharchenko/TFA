using TFA.Forums.Storage.Interfaces;

namespace TFA.Forums.Storage.Helpers;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}