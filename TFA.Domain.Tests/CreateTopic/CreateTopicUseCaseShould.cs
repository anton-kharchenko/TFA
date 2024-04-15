using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Enums;
using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.Interfaces.Storages.Topic;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateTopic;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Tests.CreateTopic;

public class CreateTopicUseCaseShould
{
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> _createTopicSetup;
    private readonly ISetup<IIdentity, Guid> _getCurrentUserIdSetup;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> _getForumsSetup;
    private readonly ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> _intentionManager = new();
    private readonly Mock<ICreateTopicStorage> _storage = new();
    private readonly CreateTopicUseCase _sut;

    public CreateTopicUseCaseShould()
    {
        _createTopicSetup = _storage.Setup(s =>
            s.CreateTopicAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var getForumsStorage = new Mock<IGetForumsStorage>();
        _getForumsSetup = getForumsStorage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        _getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        _intentionIsAllowedSetup = _intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntentionType>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        _sut = new CreateTopicUseCase(_intentionManager.Object, identityProvider.Object, getForumsStorage.Object, unitOfWork.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("3BB52FCF-FA8F-4DA3-9954-25A67F75B71A");

        _intentionIsAllowedSetup.Returns(false);

        await _sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        _intentionManager.Verify(m => m.IsAllowed(TopicIntentionType.Create));
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");

        _intentionIsAllowedSetup.Returns(true);
        _getForumsSetup.ReturnsAsync(Array.Empty<Forum>());

        await _sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Some title"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");

        _intentionIsAllowedSetup.Returns(true);
        _getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = forumId } });
        _getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic();
        _createTopicSetup.ReturnsAsync(expected);

        var actual = await _sut.Handle(new CreateTopicCommand(forumId, "Hello world"), CancellationToken.None);
        actual.Should().Be(expected);

        _storage.Verify(s => s.CreateTopicAsync(forumId, userId, "Hello world", It.IsAny<CancellationToken>()),
            Times.Once);
    }
}