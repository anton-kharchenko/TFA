using TFA.Search.Domain.Contracts;
using TFA.Search.Domain.Responses;

namespace TFA.Search.Storage.Storages;

internal class SearchStorage : ISearchStorage
{
    public Task<(IEnumerable<SearchResponse> resources, int totalCount)> SearchAsync(string query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}