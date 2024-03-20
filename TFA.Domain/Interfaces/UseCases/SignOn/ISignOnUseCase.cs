using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.UseCases.SignOn;

namespace TFA.Domain.Interfaces.UseCases.SignOn;

public interface ISignOnUseCase
{
    Task<IIdentity> ExecuteAsync(SignOnCommand command, CancellationToken cancellationToken);
}