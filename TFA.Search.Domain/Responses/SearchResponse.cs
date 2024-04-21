using TFA.Search.Domain.Enums;

namespace TFA.Search.Domain.Responses;

public class SearchResponse
{
    public Guid EntityId { get; set; }

    public SearchEntityType SearchEntityType { get; set; }

    public ICollection<string> Highlights { get; set; } = default!;
}