namespace TFA.Api.Authentication;

internal class AuthenticationTokenStorage : ITokenStorage
{
    private const string HeaderKey = "TFA-Authentication-Token";

    public bool TryExtract(HttpContext httpContext, out string token)
    {
        if (httpContext.Request.Cookies.TryGetValue(HeaderKey, out var value) &&
            !string.IsNullOrWhiteSpace(value))
        {
            token = value;
            return true;
        }

        token = string.Empty;
        return false;
    }

    public void Store(HttpContext httpContext, string token) =>
        httpContext.Response.Cookies.Append(HeaderKey, token, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true
        });
}