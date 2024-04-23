using MediatR;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Queries.GetForum;
using Forum = TFA.Forums.Domain.Models.Forum;

namespace TFA.Forums.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage) :
     IRequestHandler<GetForumQuery, IEnumerable<Models.Forum>>
{
    public async Task<IEnumerable<Models.Forum>> Handle(GetForumQuery query, CancellationToken cancellationToken) => 
        await getForumsStorage.GetForumsAsync(cancellationToken);
}