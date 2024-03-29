using System.Net.Http.Json;
using FluentAssertions;
using TFA.Storage.Entities;

namespace TFA.E2E;

public class AccountEndpointsShould(ForumApiApplicationFactory applicationFactory) : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = applicationFactory.CreateClient();

        var signOnResponse = await httpClient.PostAsync("account", JsonContent.Create(new { loggin = "Test", password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        
        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

        var signInResponse = await httpClient.PostAsync("account/signin", JsonContent.Create(new { loggin = "Test", password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();
        signInResponse.Headers.Should().ContainKey("TFA-Auth-Token");
        
        var signedUser = await signInResponse.Content.ReadFromJsonAsync<User>();

        signedUser.Should().NotBeNull().And.BeEquivalentTo(createdUser);
    }
}