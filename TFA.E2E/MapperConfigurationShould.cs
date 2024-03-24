using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TFA.E2E;

public class MapperConfigurationShould(WebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public void ShouldBeValid()
    {
        webApplicationFactory.Services.GetRequiredService<IMapper>()
            .ConfigurationProvider.Invoking(p => p.AssertConfigurationIsValid())
            .Should().NotThrow();
    }
}