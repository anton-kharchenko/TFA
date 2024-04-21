namespace TFA.Forum.Domain.Interfaces.Authentication;

public interface IIdentityProvider
{
   public IIdentity Current { get; set; }
}
