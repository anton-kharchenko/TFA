namespace TFA.Domain.Interfaces.Authentication;

internal interface IIdentityProvider
{
   public IIdentity Current { get; }
}