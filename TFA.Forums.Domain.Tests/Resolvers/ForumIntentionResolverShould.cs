using FluentAssertions;
using Moq;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Enums;
using TFA.Forums.Domain.Interfaces.Authentication;
using TFA.Forums.Domain.Resolvers;

namespace TFA.Forums.Domain.Tests.Resolvers;

public class ForumIntentionResolverShould
{
    private readonly ForumIntentionResolver sut = new();

    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        sut.IsAllowed(new Mock<IIdentity>().Object, (ForumIntentionType)(-1)).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WhenCheckingForumCreateIntention_AndUserIsGuest()
    {
        sut.IsAllowed(User.Guest, ForumIntentionType.Create).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenCheckingForumCreateIntention_AndUserIsAuthenticated()
    {
        sut.IsAllowed(new User(Guid.Parse("6fb5ab53-91c3-4444-adfd-a87713f3b94a"), Guid.Empty),
            ForumIntentionType.Create).Should().BeTrue();
    }
}