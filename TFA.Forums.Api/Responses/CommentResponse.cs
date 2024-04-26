namespace TFA.Forums.Api.Responses;

public class CommentResponse
{
    public int Id { get; set; }
    
    public string Title { get; set; } = default!;
    
    public DateTimeOffset CreateAt { get; set; }
    
    public string AuthorLogin { get; set; } = default!;
}