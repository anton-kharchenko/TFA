using TFA.Search.Domain.Enums;

namespace TFA.Search.Domain.Models;

public class SearchEntity
{
    public Guid EntityId { get; set; }
    
    public SearchEntityType SearchEntityType { get; set; }

    public string? Title { get; set; }
    
    public string? Text { get; set; }
}