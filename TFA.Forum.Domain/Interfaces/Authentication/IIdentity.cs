namespace TFA.Forum.Domain.Interfaces.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
    Guid SessionId { get; set; }
}