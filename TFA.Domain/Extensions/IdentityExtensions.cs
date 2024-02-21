using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Extensions;

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}