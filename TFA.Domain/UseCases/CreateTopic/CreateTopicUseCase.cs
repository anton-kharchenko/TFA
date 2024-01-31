using TFA.Domain.Exceptions;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase(ICreateTokenStorage storage) : ICreateTopicUseCase
{
    public async Task<Models.Topic> ExecuteAsync(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
    {
        var forumExists = await storage.ForumExistsAsync(forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        return await storage.CreateTopicAsync(forumId, authorId, title, cancellationToken);
    }
}