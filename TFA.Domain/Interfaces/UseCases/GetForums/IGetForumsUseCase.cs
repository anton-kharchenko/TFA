using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.GetForums;

public interface IGetForumsUseCase
{
    Task<IEnumerable<Forum>> ExecuteAsync(CancellationToken cancellationToken);
}