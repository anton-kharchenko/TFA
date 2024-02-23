using FluentAssertions;
using FluentValidation;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetTopic;

namespace TFA.Domain.Tests.GetTopic;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase _sut;
    private readonly Mock<IGetTopicsStorage> _getTopicsStorage;
    private ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> _getTopicsSetup;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator.Setup(v =>
            v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()));

        _getTopicsStorage = new Mock<IGetTopicsStorage>();
        _getTopicsSetup = _getTopicsStorage.Setup(s =>
            s.GetTopicsAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(validator.Object, _getTopicsStorage.Object);
    }

    [Fact]
    public async Task ExtractTopicsFromStorage()
    {
        var forumId = Guid.Parse("27d41f5e-e344-48fc-99d6-1c6fe84d0b64");
        var expectedResources = new Topic[]
        {
            new()
            {
                Title = string.Empty
            }
        };
        const int expectedTotalCount = 6;

        _getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));
        
        var (actualResources, actualTotalCount) = await _sut.ExecuteAsync(new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        _getTopicsStorage.Verify(s=>s.GetTopicsAsync(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}