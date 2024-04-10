using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Models;
using TFA.Domain.UseCases.CreateForum;

namespace TFA.Domain.Tests.CreateForum;

public class CreateForumUseCaseShould
{
    private readonly ISetup<ICreateForumStorage, Task<Forum>> _createForumSetup;
    private readonly Mock<ICreateForumStorage> _storage;
    private readonly CreateForumUseCase sut;

    public CreateForumUseCaseShould()
    {
        var intentionManager = new Mock<IIntentionManager>();
        intentionManager.Setup(m => m.IsAllowed(It.IsAny<ForumIntentionType>())).Returns(true);

        _storage = new Mock<ICreateForumStorage>();
        _createForumSetup = _storage.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        sut = new CreateForumUseCase(intentionManager.Object, _storage.Object);
    }

    [Fact]
    public async Task ReturnCreateForum()
    {
        var forum = new Forum
        {
            Id = Guid.Parse("0eb26ab4-2061-4758-8ab3-4f0f30e950cc"),
            Title = "Hi"
        };

        _createForumSetup.ReturnsAsync(forum);

        var actual = await sut.Handle(new CreateForumCommand("Hi"), CancellationToken.None);
        actual.Should().Be(forum);

        _storage.Verify(s => s.CreateAsync("Hi", It.IsAny<CancellationToken>()));
        _storage.VerifyNoOtherCalls();
    }
}