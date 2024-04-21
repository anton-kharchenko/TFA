using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Forum.Domain.Authentication;

public class User(Guid userId, Guid sessionId) : IIdentity
{
    public Guid UserId { get; } = userId;
    public Guid SessionId { get; set; } = sessionId;

    public static User Guest => new(Guid.Empty, Guid.Empty);
}