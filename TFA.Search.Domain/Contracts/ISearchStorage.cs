using TFA.Search.Domain.Responses;

namespace TFA.Search.Domain.Contracts;

public interface ISearchStorage
{
    Task<(IEnumerable<SearchResponse> resources, int totalCount)> SearchAsync(
        string query, CancellationToken cancellationToken);
}