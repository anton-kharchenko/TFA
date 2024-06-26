﻿using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Forums.Domain.Exceptions;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Queries.GetTopics;
using TFA.Forums.Domain.UseCases.GetTopic;

namespace TFA.Forums.Domain.Tests.GetTopic;

public class GetTopicsUseCaseShould
{
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Models.Forum>>> _getForumSetUp;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicsSetup;
    private readonly Mock<IGetTopicsStorage> _getTopicsStorage;
    private readonly GetTopicsUseCase _sut;

    public GetTopicsUseCaseShould()
    {
        _getTopicsStorage = new Mock<IGetTopicsStorage>();
        _getTopicsSetup = _getTopicsStorage.Setup(s =>
            s.GetTopicsAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        var getForumStorage = new Mock<IGetForumsStorage>();
        _getForumSetUp = getForumStorage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(_getTopicsStorage.Object, getForumStorage.Object);
    }

    [Fact]
    public async Task ExtractTopicsFromStorage()
    {
        var forumId = Guid.Parse("27d41f5e-e344-48fc-99d6-1c6fe84d0b64");
        var expectedResources = new Topic[] { new() { Title = string.Empty } };
        const int expectedTotalCount = 6;

        _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) =
            await _sut.Handle(new GetTopicQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _getTopicsStorage.Verify(s => s.GetTopicsAsync(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenForumNotFound()
    {
        var forumId = Guid.Parse("0b6aaa4e-c36a-4353-8285-a6b8bcb3823d");

        _getForumSetUp.ReturnsAsync(new Models.Forum[] { new() { Id = Guid.Parse("6135ef52-95af-4617-b1a9-86da3d02da2b") } });

        var query = new GetTopicQuery(forumId, 0, 1);
        await _sut.Invoking(s => s.Handle(query, CancellationToken.None))
            .Should()
            .ThrowAsync<ForumNotFoundException>();
    }
}