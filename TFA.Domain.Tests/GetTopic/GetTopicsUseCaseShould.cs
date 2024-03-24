using FluentAssertions;
using FluentValidation;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetTopic;

namespace TFA.Domain.Tests.GetTopic;

public class GetTopicsUseCaseShould
{
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumSetUp;
    private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicsSetup;
    private readonly Mock<IGetTopicsStorage> _getTopicsStorage;
    private readonly GetTopicsUseCase _sut;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator.Setup(v =>
            v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()));

        _getTopicsStorage = new Mock<IGetTopicsStorage>();
        _getTopicsSetup = _getTopicsStorage.Setup(s =>
            s.GetTopicsAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        var getForumStorage = new Mock<IGetForumsStorage>();
        _getForumSetUp = getForumStorage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(validator.Object, _getTopicsStorage.Object, getForumStorage.Object);
    }

    [Fact]
    public async Task ExtractTopicsFromStorage()
    {
        var forumId = Guid.Parse("27d41f5e-e344-48fc-99d6-1c6fe84d0b64");
        var expectedResources = new Topic[] { new() { Title = string.Empty } };
        const int expectedTotalCount = 6;

        _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) =
            await _sut.ExecuteAsync(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _getTopicsStorage.Verify(s => s.GetTopicsAsync(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenForumNotFound()
    {
        var forumId = Guid.Parse("0b6aaa4e-c36a-4353-8285-a6b8bcb3823d");

        _getForumSetUp.ReturnsAsync(new Forum[] { new() { Id = Guid.Parse("6135ef52-95af-4617-b1a9-86da3d02da2b") } });

        var query = new GetTopicsQuery(forumId, 0, 1);
        await _sut.Invoking(s => s.ExecuteAsync(query, CancellationToken.None))
            .Should()
            .ThrowAsync<ForumNotFoundException>();
    }
}