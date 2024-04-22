using OpenSearch.Client;
using TFA.Search.Domain.Contracts;
using TFA.Search.Domain.Enums;
using TFA.Search.Domain.Responses;
using TFA.Search.Storage.Entities;

namespace TFA.Search.Storage.Storages;

internal class SearchStorage(IOpenSearchClient client) : ISearchStorage
{
    public async Task<(IEnumerable<SearchResponse> resources, int totalCount)> SearchAsync(string query,
        CancellationToken cancellationToken)
    {
        var searchResponse = await client.SearchAsync<SearchEntity>(desc => desc
                .Query(q => q
                    .Bool(b => b
                        .Should(s => s
                                .Match(m => m
                                    .Field(se => se.Title)
                                    .Query(query)),
                            s => s
                                .Match(m => m
                                    .Field(se => se.Text)
                                    .Query(query)
                                    .Fuzziness(Fuzziness.EditDistance(1)))))
                ).Highlight(h => h
                    .Fields(f =>
                        f.Field(se => se.Title), f =>
                        f.Field(se => se.Text).PreTags("<mark>").PostTags("</mark>"))),
            cancellationToken);

        var searchResponses = searchResponse.Hits.Select(hit => new SearchResponse()
        {
            EntityId = hit.Source.EntityId,
            SearchEntityType = (SearchEntityType)hit.Source.EntityType,
            Highlights = hit.Highlight.Values.SelectMany(v => v).ToArray()
        });

        return (searchResponses, (int)searchResponse.Total);
    }
}