namespace TFA.Forums.Storage.Entities;

public class TopicListItemReadModel
{
    public Guid TopicId { get; set; }

    public Guid ForumId { get; set; }
    
    public Guid UserId { get; set; }

    public DateTimeOffset CreateAt { get; set; }

    public string Title { get; set; } = default!;

    public int TotalCommentsCount { get; set; }

    public DateTimeOffset? LastCommentCreateAt { get; set; }

    public Guid? LastCommentId { get; set; }
}