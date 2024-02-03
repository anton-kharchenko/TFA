using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages;
using TFA.Domain.UseCases.CreateTopic;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    
    private readonly Mock<ICreateTopicStorage> storage;
    private readonly Mock<IIntentionManager> _intentionManager;
    
    private readonly ISetup<ICreateTopicStorage, Task<bool>> _forumExistsSetup;
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> _createTopicSetup;
    private readonly ISetup<IIdentity, Guid> _getCurrentUserId;
    private readonly ISetup<IIntentionManager, bool> _intentionIsAllowedSetup;

    public CreateTopicUseCaseShould()
    {
        storage = new Mock<ICreateTopicStorage>();
        
        _intentionManager = new Mock<IIntentionManager>();
        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        
        _forumExistsSetup = storage.Setup(s => s.ForumExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _createTopicSetup = storage.Setup(s => s.CreateTopicAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
        identityProvider.Setup(i => i.Current).Returns(identity.Object);
        _getCurrentUserId = identity.Setup(s => s.UserId);
        
        _intentionIsAllowedSetup  = _intentionManager.Setup(s=>s.IsAllowed(It.IsAny<TopicIntention>()));
       
        sut = new CreateTopicUseCase(_intentionManager.Object, storage.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationNotAllowed()
    {
        var forumId = Guid.Parse("7340cea0-e93c-465a-af1e-55d9c132d687");

        _intentionIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.ExecuteAsync(forumId, "", CancellationToken.None)).Should()
            .ThrowAsync<IntentionManagerException>();
        
        _intentionManager.Verify(m=>m.IsAllowed(TopicIntention.Create));
    }

    [Fact]
    public async Task ThrowFoundNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("00749cdb-b557-4c3d-952d-a72797edd996");

        _intentionIsAllowedSetup.Returns(true);
        _forumExistsSetup.ReturnsAsync(false);

        await sut.Invoking(s => s.ExecuteAsync(forumId, "title", CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();

        storage.Verify(s => s.ForumExistsAsync(forumId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("c789d8ae-ebe7-4f1b-a2d8-c49ffb7b641c");
        var userId = Guid.Parse("872d2338-64a9-4161-b403-0d72cbe984b4");

        _intentionIsAllowedSetup.Returns(true);
        _forumExistsSetup.ReturnsAsync(true);
        _getCurrentUserId.Returns(userId);

        var expectedTopic = new Topic();
        _createTopicSetup.ReturnsAsync(expectedTopic);

        const string title = "Hello world";

        var actual = await sut.ExecuteAsync(forumId, title, CancellationToken.None);

        actual.Should().BeEquivalentTo(expectedTopic);

        storage.Verify(s => s.CreateTopicAsync(forumId, userId, title, It.IsAny<CancellationToken>()), Times.Once);
    }
}