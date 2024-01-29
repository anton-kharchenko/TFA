using TFA.Storage;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private ForumDbContext _dbContext;

    public CreateTopicUseCase(ForumDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Topic> ExecuteAsync(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}