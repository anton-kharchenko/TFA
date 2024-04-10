using System.Net;
using FluentAssertions;
using Moq;
using TFA.Domain.Authentication;
using TFA.Domain.Authorization;
using TFA.Domain.Enums;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;

namespace TFA.Domain.Tests.Authorization;

public class IntentionManagerShould
{
    [Fact]
    public void ReturnFalse_WhenNoMatchingResolver()
    {
        var resolvers = new[]
        {
            new Mock<IIntentionResolver>().Object,
            new Mock<IIntentionResolver<HttpStatusCode>>().Object
        };

        var sut = new IntentionManager(resolvers, new Mock<IIdentityProvider>().Object);

        sut.IsAllowed(ForumIntentionType.Create).Should().BeFalse();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void ReturnMatchingResolverResult(bool expectedResolverResult, bool expected)
    {
        var intentionResolver = new Mock<IIntentionResolver<ForumIntentionType>>();
        intentionResolver
            .Setup(r => r.IsAllowed(It.IsAny<IIdentity>(), It.IsAny<ForumIntentionType>()))
            .Returns(expectedResolverResult);

        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider
            .Setup(p => p.Current)
            .Returns(new User(Guid.Parse("ad3ad09d-84e4-4c83-b398-a20a0c4c5e70"), Guid.Empty));

        var resolvers = new IIntentionResolver[] { intentionResolver.Object };

        var sut = new IntentionManager(resolvers, identityProvider.Object);

        sut.IsAllowed(ForumIntentionType.Create).Should().Be(expected);
    }
}