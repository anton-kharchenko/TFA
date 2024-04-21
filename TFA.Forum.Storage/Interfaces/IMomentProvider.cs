namespace TFA.Forum.Storage.Interfaces;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}