using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TFA.E2E;

public class ForumEndpointsShould(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    
    [Fact]
    public async Task ReturnListOfForums()
    {
      using var httpClient  = factory.CreateClient();
      using var response =  await httpClient.GetAsync("forums");
      response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
      var result = await response.Content.ReadAsStringAsync();
      result.Should().Be("[]");
    }
}