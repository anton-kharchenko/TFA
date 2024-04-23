namespace TFA.Forums.Domain.Interfaces.Authentication;

public interface IIdentityProvider
{
   public IIdentity Current { get; set; }
}
