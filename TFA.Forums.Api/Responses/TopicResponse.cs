namespace TFA.Forums.Api.Responses;

public class TopicResponse
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}