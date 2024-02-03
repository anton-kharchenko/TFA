using TFA.Domain.Authentication;
using TFA.Domain.Interfaces;
using TFA.Domain.Interfaces.Authentication;

namespace TFA.Domain.Extensions;

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}