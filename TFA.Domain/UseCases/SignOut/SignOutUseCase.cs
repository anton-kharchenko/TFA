using TFA.Domain.Commands.SignOut;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.UseCases.SignOut;

namespace TFA.Domain.UseCases.SignOut;

internal class SignOutUseCase(
    IIdentityProvider identityProvider,
    ISignOutStorage signOutStorage
) : ISignOutUseCase
{
    public Task ExecuteAsync(SignOutCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}