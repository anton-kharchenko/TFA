using MediatR;
using Microsoft.AspNetCore.Mvc;
using TFA.Api.Authentication;
using TFA.Api.Requests.SignIn;
using TFA.Api.Requests.SignOn;
using TFA.Forums.Domain.Commands.SignIn;
using TFA.Forums.Domain.Commands.SignOn;

namespace TFA.Api.Controllers;

[ApiController]
[Route("account")]
public class AccountControllers(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> SignOn(
        [FromBody] SignOnRequest request,
        CancellationToken cancellationToken)
    {
        var identity = await _mediator.Send(new SignOnCommand(request.Login, request.Password), cancellationToken);
        
        return Ok(identity);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInRequest request,
        [FromServices] ITokenStorage tokenStorage,
        CancellationToken cancellationToken)
    {
        var (identity, token) = await _mediator.Send(new SignInCommand(request.Login, request.Password), cancellationToken);
            
        tokenStorage.Store(HttpContext, token);

        return Ok(identity);
    }
}