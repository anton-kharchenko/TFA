using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace TFA.E2E;

public class MapperConfigurationShould(ForumApiApplicationFactory applicationFactory)
    : IClassFixture<ForumApiApplicationFactory>
{
    [Fact]
    public void ShouldBeValid()
    {
        applicationFactory.Services.GetRequiredService<IMapper>()
            .ConfigurationProvider.Invoking(p => p.AssertConfigurationIsValid())
            .Should().NotThrow();
    }
}