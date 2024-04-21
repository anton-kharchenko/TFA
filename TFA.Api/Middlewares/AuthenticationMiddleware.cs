using TFA.Api.Authentication;
using TFA.Forum.Domain.Authentication;
using TFA.Forum.Domain.Interfaces.Authentication;

namespace TFA.Api.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        ITokenStorage authenticationStorage,
        IAuthenticationService authenticationService,
        IIdentityProvider providerSetter,
        CancellationToken cancellationToken)
    {
        var identity = authenticationStorage.TryExtract(httpContext, out var authToken)
            ? await authenticationService.AuthenticateAsync(authToken, cancellationToken)
            : User.Guest;

        providerSetter.Current = identity;

        await next(httpContext);
    }
}