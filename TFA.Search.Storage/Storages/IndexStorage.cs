using OpenSearch.Client;
using TFA.Search.Domain.Contracts;
using TFA.Search.Domain.Enums;

namespace TFA.Search.Storage.Storages;

internal class IndexStorage(IOpenSearchClient client) : IIndexStorage
{
    public async Task IndexAsync(Guid entityId, SearchEntityType entityType, string? title, string? text,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}