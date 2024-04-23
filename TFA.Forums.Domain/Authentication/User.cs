using TFA.Forums.Domain.Interfaces.Authentication;

namespace TFA.Forums.Domain.Authentication;

public class User(Guid userId, Guid sessionId) : IIdentity
{
    public Guid UserId { get; } = userId;
    public Guid SessionId { get; set; } = sessionId;

    public static User Guest => new(Guid.Empty, Guid.Empty);
}