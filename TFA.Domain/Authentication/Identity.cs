using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Authentication;

internal class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;
}