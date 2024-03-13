using TFA.Domain.Interfaces.UseCases.GetForums;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

internal class GetForumsUseCase(IGetForumsStorage getForumsStorage) : IGetForumsUseCase
{
    public async Task<IEnumerable<Forum>?> ExecuteAsync(CancellationToken cancellationToken) =>
        await getForumsStorage.GetForumsAsync(cancellationToken);
}