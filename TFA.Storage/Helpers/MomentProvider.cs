using TFA.Storage.Interfaces;

namespace TFA.Storage.Helpers;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}