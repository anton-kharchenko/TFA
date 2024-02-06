using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.UseCases.GetForums;

namespace TFA.Storage.Storages;

public class GetForumsStorage(ForumDbContext dbContext) : IGetForumsStorage
{
    public async Task<IEnumerable<Domain.Models.Forum>> GetForumsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Forums.Select(i => new Domain.Models.Forum
        {
            Id = i.ForumId,
            Title = i.Title
        }).ToArrayAsync(cancellationToken);
    }
}