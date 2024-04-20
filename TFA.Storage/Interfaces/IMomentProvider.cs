namespace TFA.Storage.Interfaces;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}