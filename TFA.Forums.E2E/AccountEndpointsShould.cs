using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Entities;

namespace TFA.Forums.E2E;

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
            JsonContent.Create(new { Title = "0123456789012345678901234567890123456789" }));
        createForumResponse.IsSuccessStatusCode.Should().BeTrue();

        await using var scope = applicationFactory.Services.CreateAsyncScope();
        var forumDbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
        (await forumDbContext.Forums.ToArrayAsync()).Should().ContainSingle(f => f.Title == testTitle);
        (await forumDbContext.Topics.ToArrayAsync()).Should().ContainSingle(f => f.Title == testTitle);
    }
}