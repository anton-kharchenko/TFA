namespace TFA.Forums.Domain.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
}