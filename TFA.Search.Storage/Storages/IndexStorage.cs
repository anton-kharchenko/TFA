using OpenSearch.Client;
using TFA.Search.Domain.Contracts;
using TFA.Search.Domain.Enums;
using TFA.Search.Storage.Entities;

namespace TFA.Search.Storage.Storages;

internal class IndexStorage(IOpenSearchClient client) : IIndexStorage
{
    public async Task IndexAsync(Guid entityId, SearchEntityType entityType, string? title, string? text,
        CancellationToken cancellationToken)
    {
        await client.IndexAsync(new SearchEntity
        {
            EntityId = entityId,
            EntityType = (int)entityType,
            Title = title,
            Text = text,
        }, desc =>
            desc.Index("tfa-search-v1"), cancellationToken);
    }
}