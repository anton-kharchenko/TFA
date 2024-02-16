namespace TFA.Domain.Commands.CreateTopic;

public class CreateTopicCommand
{
    public Guid ForumId { get; set; }

    public string Title { get; set; }
}