﻿using FluentAssertions;
using Moq;
using TFA.Forum.Domain.Authentication;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Resolvers.Forum;

namespace TFA.Forum.Domain.Tests.Resolvers;

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