namespace TFA.Domain;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}