using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Storage.Context;
using TFA.Storage.Entities;

namespace TFA.E2E;

public class AccountEndpointsShould(ForumApiApplicationFactory applicationFactory)
    : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = applicationFactory.CreateClient();

        var signOnResponse =
            await httpClient.PostAsync("account", JsonContent.Create(new { loggin = "Test", password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();

        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

        var signInResponse = await httpClient.PostAsync("account/signin",
            JsonContent.Create(new { loggin = "Test", password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();

        var signedUser = await signInResponse.Content.ReadFromJsonAsync<User>();

        signedUser!.UserId.Should().Be(createdUser!.UserId);

        const string testTitle = "Test title";
        var createForumResponse =
            await httpClient.PostAsync("forums", JsonContent.Create(new { title = testTitle }));
        createForumResponse.IsSuccessStatusCode.Should().BeTrue();

        var createdForum = await createForumResponse.Content.ReadFromJsonAsync<Forum>();

        var createTopicResponse = await httpClient.PostAsync($"forums/{createdForum!.ForumId}/topics",
            JsonContent.Create(new { Title = "New topic" }));
        createForumResponse.IsSuccessStatusCode.Should().BeTrue(await createTopicResponse.Content.ReadAsStringAsync());

        await using var scope = applicationFactory.Services.CreateAsyncScope();
        var forumDbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
        var domainEvents = await forumDbContext.DomainEvents.ToArrayAsync();
        domainEvents.Should().HaveCount(1);
        var topic = JsonSerializer.Deserialize<Topic>(domainEvents[0].ContentBlob);
        topic.Should().NotBeNull();
        topic!.Title.Should().Be(testTitle);
    }
}