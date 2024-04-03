using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Monitoring;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage, DomainMetrics metrics) : IGetForumsUseCase
{
    public async Task<IEnumerable<Forum>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await getForumsStorage.GetForumsAsync(cancellationToken);
            metrics.ForumsFetched(true);
            return result;
        }
        catch
        {
            metrics.ForumsFetched(false);
            throw new Exception();
        }
    }
}