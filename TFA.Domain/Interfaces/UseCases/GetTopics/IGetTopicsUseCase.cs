using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Models;

namespace TFA.Domain.Interfaces.UseCases.GetTopics;

public interface IGetTopicsUseCase
{
    Task<(IEnumerable<Topic> resources, int totalCount)> ExecuteAsync(GetTopicsQuery query,
        CancellationToken cancellationToken);
}