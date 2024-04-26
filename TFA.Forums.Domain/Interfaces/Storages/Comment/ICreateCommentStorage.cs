namespace TFA.Forums.Domain.Interfaces.Storages.Comment
{
    public interface ICreateCommentStorage : IStorage
    {
        Task<Models.Comment> CreateAsync(Guid topicId, Guid userId, string text, CancellationToken cancellationToken);
        
        Task<Models.Topic?> FindTopicAsync(Guid topicId, CancellationToken cancellationToken);
    }
}