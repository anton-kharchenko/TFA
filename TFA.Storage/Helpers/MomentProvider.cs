namespace TFA.Storage.Helpers;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}