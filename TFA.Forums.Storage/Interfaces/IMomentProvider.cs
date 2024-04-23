namespace TFA.Forums.Storage.Interfaces;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}