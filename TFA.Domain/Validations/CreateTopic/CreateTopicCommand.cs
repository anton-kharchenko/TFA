namespace TFA.Domain.Validations.CreateTopic;

public record CreateTopicCommand(Guid ForumId, string Title);