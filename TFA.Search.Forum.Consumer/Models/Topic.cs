namespace TFA.Search.Forum.Consumer.Models;

public class Topic
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = default!;
}