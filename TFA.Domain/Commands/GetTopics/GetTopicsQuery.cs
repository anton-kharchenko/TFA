namespace TFA.Domain.Commands.GetTopics;

public record GetTopicsQuery(Guid ForumId, int Skip, int Take);