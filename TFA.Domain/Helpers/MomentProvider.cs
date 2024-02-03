using TFA.Domain.Interfaces.Helpers;

namespace TFA.Domain.Helpers;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}