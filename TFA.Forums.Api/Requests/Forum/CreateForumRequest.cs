namespace TFA.Forums.Api.Requests.Forum;

public class CreateForumRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
}