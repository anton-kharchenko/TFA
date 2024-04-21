using MediatR;
using TFA.Search.Domain.Responses;

namespace TFA.Search.Domain.Queries;

public record SearchQuery(string Query) : IRequest<(IEnumerable<SearchResponse> , int totalCount)>;