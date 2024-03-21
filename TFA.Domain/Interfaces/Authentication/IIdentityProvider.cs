namespace TFA.Domain.Interfaces.Authentication;

public interface IIdentityProvider
{
   public IIdentity Current { get; set; }
}
