using TFA.Forums.Domain.Interfaces.Authentication;

namespace TFA.Forums.Domain.Extensions;

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity)
    {
        return identity.UserId != Guid.Empty;
    }
}