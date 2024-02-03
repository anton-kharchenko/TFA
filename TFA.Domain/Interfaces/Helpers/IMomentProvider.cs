namespace TFA.Domain.Interfaces.Helpers;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}