namespace TFA.Api.Authentication;

internal class AuthenticationTokenStorage : ITokenStorage
{
    private const string HeaderKey = "TFA-Authentication-Token";
    public bool TryExtract(HttpContext httpContext, out string token)
    {
        if (httpContext.Request.Headers.TryGetValue(HeaderKey, out var values) &&
            !string.IsNullOrWhiteSpace(values.FirstOrDefault()))
        {
            token = values.First()!;
            return true;
        }
        
        token = string.Empty;
        return false;
    }

    public void Store(HttpContext httpContext, string token) => httpContext.Response.Headers[HeaderKey] = token;
}