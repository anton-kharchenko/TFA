using TFA.Domain.Commands.SignIn;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.UseCases.SignIn;

namespace TFA.Domain.Interfaces.UseCases.SignIn;

public interface ISignInUseCase
{
    Task<(IIdentity identity, string token)> ExecuteAsync(SignInCommand command, CancellationToken cancellationToken);
}