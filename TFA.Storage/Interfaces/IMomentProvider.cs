namespace TFA.Storage.Interfaces;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}