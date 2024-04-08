using MediatR;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Queries.GetForum;
using Forum = TFA.Domain.Models.Forum;

namespace TFA.Domain.UseCases.GetForums;

public class GetForumsUseCase(IGetForumsStorage getForumsStorage) : IRequestHandler<GetForumQuery, IEnumerable<Forum>>
{
    public async Task<IEnumerable<Forum>> Handle(GetForumQuery query, CancellationToken cancellationToken) => 
        await getForumsStorage.GetForumsAsync(cancellationToken);
}