using MediatR;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Monitoring;
using TFA.Domain.Queries.GetForum;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage, DomainMetrics metrics) : IRequestHandler<GetForumQuery, IEnumerable<Forum>>
{
    public async Task<IEnumerable<Forum>> Handle(GetForumQuery query, CancellationToken cancellationToken)
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