namespace TFA.Domain.Interfaces.Authentication;

internal interface IIdentity
{
    Guid UserId { get; }
}