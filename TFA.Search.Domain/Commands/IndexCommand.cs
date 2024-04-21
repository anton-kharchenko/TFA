using TFA.Search.Domain.Enums;

namespace TFA.Search.Domain.Commands;

public record IndexCommand(Guid EntityId, SearchEntityType SearchEntityType, string? Title, string Text);