using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.GetForums;

internal interface IGetForumsUseCase
{
    Task<IEnumerable<Forum>?> ExecuteAsync(CancellationToken cancellationToken);
}