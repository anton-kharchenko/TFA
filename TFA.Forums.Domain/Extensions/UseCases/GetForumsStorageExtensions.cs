using TFA.Forums.Domain.Exceptions;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;

namespace TFA.Forums.Domain.Extensions.UseCases;

public static class GetForumsStorageExtensions
{
    private static async Task<bool> ForumExistsAsync(this IGetForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        var forums = await storage.GetForumsAsync(cancellationToken);
        return forums.Any(f => f.Id == forumId);
    }

    public static async Task ThrowIfForumNotFoundAsync(this IGetForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        if (!await ForumExistsAsync(storage, forumId, cancellationToken)) throw new ForumNotFoundException(forumId);
    }
}