using TFA.Domain.Commands.SignOut;

namespace TFA.Domain.Interfaces.UseCases.SignOut;

public interface ISignOutUseCase
{
    Task ExecuteAsync(SignOutCommand command, CancellationToken cancellationToken);
}