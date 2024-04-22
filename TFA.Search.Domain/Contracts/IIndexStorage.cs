using TFA.Search.Domain.Enums;

namespace TFA.Search.Domain.Contracts;

public interface IIndexStorage
{
    Task IndexAsync(Guid entityId, SearchEntityType entityType, string? title, string? text,
        CancellationToken cancellationToken);
}