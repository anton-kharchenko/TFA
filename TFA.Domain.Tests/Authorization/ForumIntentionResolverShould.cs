using FluentAssertions;
using Moq;
using TFA.Domain.Authentication;
using TFA.Domain.Enums.Forum;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Resolvers.Forum;

namespace TFA.Domain.Tests.Authorization;

public class ForumIntentionResolverShould
{
    private readonly ForumIntentionResolver sut = new();
    
    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum() => 
        sut.IsAllowed(new Mock<IIdentity>().Object, (ForumIntention)(-1)).Should().BeFalse();

    [Fact]
    public void ReturnFalse_WhenCheckingForumCreateIntention_AndUserIsGuest() => 
        sut.IsAllowed(User.Guest, ForumIntention.Create).Should().BeFalse();

    [Fact]
    public void ReturnTrue_WhenCheckingForumCreateIntention_AndUserIsAuthenticated() => 
        sut.IsAllowed(new User(Guid.Parse("6fb5ab53-91c3-4444-adfd-a87713f3b94a"), Guid.Empty),  ForumIntention.Create).Should().BeTrue();
}