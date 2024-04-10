using FluentAssertions;
using Moq;
using TFA.Domain.Authentication;
using TFA.Domain.Enums;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Resolvers.SignOut;

namespace TFA.Domain.Tests.Resolvers;

public class AccountIntentionResolverShould
{
    private readonly AccountIntentionResolver sut = new();

    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        sut.IsAllowed(new Mock<IIdentity>().Object, (AccountIntentionType)(-1)).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WhenSignOut_AndUserIsGuest()
    {
        sut.IsAllowed(User.Guest, AccountIntentionType.SignOut).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenSignOut_AndUserIsAuthenticated()
    {
        sut.IsAllowed(new User(Guid.Parse("6fb5ab53-91c3-4444-adfd-a87713f3b94a"), Guid.Empty),
            AccountIntentionType.SignOut).Should().BeTrue();
    }
}