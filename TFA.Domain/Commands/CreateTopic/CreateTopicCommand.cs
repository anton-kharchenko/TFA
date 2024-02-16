namespace TFA.Domain.Commands.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title);