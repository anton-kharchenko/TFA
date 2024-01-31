using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    private readonly Mock<ICreateTokenStorage> storage;
    private readonly ISetup<ICreateTokenStorage, Task<bool>> forumExistsSetup;
    private readonly ISetup<ICreateTokenStorage, Task<Topic>> createTopicSetup;

    public CreateTopicUseCaseShould()
    {
        storage = new Mock<ICreateTokenStorage>();
        forumExistsSetup = storage.Setup(s => s.ForumExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        createTopicSetup = storage.Setup(s =>
            s.CreateTopicAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        sut = new CreateTopicUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowFoundNotFoundException_WhenNoMatchingForum()
    {
        forumExistsSetup.ReturnsAsync(false);

        var forumId = Guid.Parse("00749cdb-b557-4c3d-952d-a72797edd996");
        var authorId = Guid.Parse("774d3a63-3938-469b-8406-9cd9ec8fe7be");

        await sut.Invoking(s => s.ExecuteAsync(forumId, "title", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();

        storage.Verify(s => s.ForumExistsAsync(forumId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        forumExistsSetup.ReturnsAsync(true);
    
        var expectedTopic = new Topic();
        createTopicSetup.ReturnsAsync(expectedTopic);
        
        var forumId = Guid.Parse("c789d8ae-ebe7-4f1b-a2d8-c49ffb7b641c");
        var userId = Guid.Parse("872d2338-64a9-4161-b403-0d72cbe984b4");
        const string title = "Hello world";

        var actual = await sut.ExecuteAsync(forumId, title, userId, CancellationToken.None);
        
        actual.Should().BeEquivalentTo(expectedTopic);
        
        storage.Verify(s=>s.CreateTopicAsync(forumId, userId, title, It.IsAny<CancellationToken>()), Times.Once);
    }
}