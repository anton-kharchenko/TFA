using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TFA.Api.Requests;
using TFA.Api.Requests.Forum;

namespace TFA.E2E;

public class TopicEndpointsShould(ForumApiApplicationFactory factory)
    : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    private readonly Guid forumId = new Guid();

    private const string ForumTitle = "Forum Title";

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ReturnForbidden_WhenNotAuthenticated()
    {
        using var httpClient = factory.CreateClient();

        using var forum = await httpClient.PostAsync("forums",
            JsonContent.Create(new { title = ForumTitle }));
            
        forum.EnsureSuccessStatusCode();

       var createdForumRequest = await forum.Content.ReadFromJsonAsync<CreateForumRequest>();
       createdForumRequest.Should().NotBeNull();
        
        var topics = await httpClient.PostAsync($"forums/{createdForumRequest!.Id}/topics",
            JsonContent.Create(new { title = "hello world" }));

        topics.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}