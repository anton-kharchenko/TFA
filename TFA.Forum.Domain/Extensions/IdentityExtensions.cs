using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Forum.Domain.Extensions;

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity)
    {
        return identity.UserId != Guid.Empty;
    }
}