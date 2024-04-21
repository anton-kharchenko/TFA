using MediatR;
using TFA.Search.Domain.Queries;
using TFA.Search.Domain.Responses;

namespace TFA.Search.Domain.UseCases.Search;

public class SearchUseCase : IRequestHandler<SearchQuery, (IEnumerable<SearchResponse> , int totalCount)>
{
    public Task<(IEnumerable<SearchResponse>, int totalCount)> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}