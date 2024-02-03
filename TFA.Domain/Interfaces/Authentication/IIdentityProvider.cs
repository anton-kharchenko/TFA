namespace TFA.Domain.Interfaces.Authentication;

public interface IIdentityProvider
{
    IIdentity Current { get; }
}