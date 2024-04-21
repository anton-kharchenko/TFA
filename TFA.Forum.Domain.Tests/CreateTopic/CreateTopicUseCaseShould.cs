using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Domain.Commands.CreateTopic;
using TFA.Forum.Domain.Enums;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.Interfaces.Authentication;
using TFA.Forum.Domain.Interfaces.Authorization;
using TFA.Forum.Domain.Interfaces.Storages;
using TFA.Forum.Domain.Interfaces.Storages.Topic;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.UseCases.CreateTopic;
using Topic = TFA.Forum.Domain.Models.Topic;

namespace TFA.Forum.Domain.Tests.CreateTopic;

public class CreateTopicUseCaseShould
{
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
    private readonly ISetup<IIdentity, Guid> getCurrentUserIdSetup;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Models.Forum>>> getForumsSetup;
    private readonly ISetup<IIntentionManager, bool> intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> intentionManager = new();
    private readonly Mock<ICreateTopicStorage> storage = new();
    private readonly CreateTopicUseCase sut;

    public CreateTopicUseCaseShould()
    {
        createTopicSetup = storage.Setup(s =>
            s.CreateTopicAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var getForumsStorage = new Mock<IGetForumsStorage>();
        getForumsSetup = getForumsStorage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        intentionIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntentionType>()));

        var unitOfWork = new Mock<IUnitOfWork>();
        sut = new CreateTopicUseCase(intentionManager.Object,  identityProvider.Object, getForumsStorage.Object, unitOfWork.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("3BB52FCF-FA8F-4DA3-9954-25A67F75B71A");

        intentionIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Whatever"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        intentionManager.Verify(m => m.IsAllowed(TopicIntentionType.Create));
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(Array.Empty<Models.Forum>());

        await sut.Invoking(s => s.Handle(new CreateTopicCommand(forumId, "Some title"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(new Models.Forum[] { new() { Id = forumId } });
        getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic();
        createTopicSetup.ReturnsAsync(expected);

        var actual = await sut.Handle(new CreateTopicCommand(forumId, "Hello world"), CancellationToken.None);
        actual.Should().Be(expected);

        storage.Verify(s => s.CreateTopicAsync(forumId, userId, "Hello world", It.IsAny<CancellationToken>()),
            Times.Once);
    }
}