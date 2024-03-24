using TFA.Domain.Interfaces.UseCases.GetForums;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage) : IGetForumsUseCase
{
    public async Task<IEnumerable<Forum>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await getForumsStorage.GetForumsAsync(cancellationToken);
    }
}