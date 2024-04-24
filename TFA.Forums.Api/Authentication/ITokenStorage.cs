namespace TFA.Forums.Api.Authentication;

public interface ITokenStorage
{
    bool TryExtract(HttpContext httpContext, out string token);

    void Store(HttpContext httpContext, string token);
}