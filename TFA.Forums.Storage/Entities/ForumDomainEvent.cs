using TFA.Forums.Storage.Enums;

namespace TFA.Forums.Storage.Entities;

public class ForumDomainEvent
{
    public ForumDomainEventType EventType { get; set; }

    public Guid TopicId { get; set; }

    public string Title { get; set; } = null!;

    public ForumComment? Comment { get; set; }
    
    public class ForumComment
    {
        public Guid CommentId { get; set; }

        public string Text { get; set; } = null!;
    }
}