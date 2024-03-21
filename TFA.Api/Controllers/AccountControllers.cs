using Microsoft.AspNetCore.Mvc;
using TFA.Api.Authentication;
using TFA.Api.Requests.SignIn;
using TFA.Api.Requests.SignOn;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Interfaces.UseCases.SignOn;
using TFA.Domain.UseCases.SignIn;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Api.Controllers;

[ApiController]
[Route("account")]
public class AccountControllers : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignOn(
        [FromBody] SignOnRequest request,
        [FromServices] ISignOnUseCase useCase,
        CancellationToken cancellationToken)
    {
        var identity = await useCase.ExecuteAsync(new SignOnCommand(request.Login, request.Password), cancellationToken);
        
        return Ok(identity);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInRequest request,
        [FromServices] ISignInUseCase useCase,
        [FromServices] ITokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        var (identity, token) = await useCase.ExecuteAsync(new SignInCommand(request.Login, request.Password), cancellationToken);
            
        tokenStorage.Store(HttpContext, token);

        return Ok(identity);
    }
}