namespace TFA.Storage.Helpers;

internal interface IMomentProvider
{
    DateTimeOffset Now { get; }
}