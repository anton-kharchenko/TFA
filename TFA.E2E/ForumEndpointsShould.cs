using System.Net.Http.Json;
using FluentAssertions;
using TFA.Domain.Models;

namespace TFA.E2E;

public class ForumEndpointsShould(ForumApiApplicationFactory factory) : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public async Task CreateNewForum()
    {
        using var httpClient = factory.CreateClient();
        using var postForumResponse = await httpClient
            .PostAsync("forums", JsonContent.Create(new { title = "test" }));

        postForumResponse
            .Invoking(r => r.EnsureSuccessStatusCode())
            .Should()
            .NotThrow();

        using var getForumResponse = await httpClient
            .GetAsync("forums");

        var forum = await getForumResponse
            .Content
            .ReadFromJsonAsync<Forum>();

        forum.Should().NotBeNull()
            .And.Subject.As<Forum>().Should().Be("Test");

        forum!.Id.Should().NotBeEmpty();    

        var forums = await getForumResponse
            .Content
            .ReadFromJsonAsync<Forum[]>();

        forums.Should().NotBeNull().And.Subject.As<Forum[]>().Should().Contain(f => f.Title == "");
    }
}