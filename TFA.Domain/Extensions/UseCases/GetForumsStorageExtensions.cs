using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.UseCases.GetForums;

namespace TFA.Domain.Extensions.UseCases;

public static class GetForumsStorageExtensions
{
    public static async Task<bool> ForumExistsAsync(this IGetForumsStorage storage, Guid forumId, CancellationToken cancellationToken)
    {
      var forums =  await storage.GetForumsAsync(cancellationToken);
      return forums.Any(f => f.Id == forumId);
    }

    public static async Task ThrowIfForumNotFoundAsync(this IGetForumsStorage storage, Guid forumId,
        CancellationToken cancellationToken)
    {
        if (!await ForumExistsAsync(storage, forumId, cancellationToken))
        {
            throw new ForumNotFoundException(forumId);
        }
    }
}