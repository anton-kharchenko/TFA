using MediatR;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Queries.GetForum;
using Forum = TFA.Forum.Domain.Models.Forum;

namespace TFA.Forum.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage) :
     IRequestHandler<GetForumQuery, IEnumerable<Models.Forum>>
{
    public async Task<IEnumerable<Models.Forum>> Handle(GetForumQuery query, CancellationToken cancellationToken) => 
        await getForumsStorage.GetForumsAsync(cancellationToken);
}