using System.Diagnostics.Metrics;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Commands.CreateForum;
using TFA.Domain.Enums;
using TFA.Domain.Interfaces.Authorization;
using TFA.Domain.Interfaces.Storages.Forum;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.Validations.CreateForum;

namespace TFA.Domain.Tests.CreateForum;

public class CreateForumUseCaseShould
{
    private readonly Mock<ICreateForumStorage> _storage;
    private readonly ISetup<ICreateForumStorage, Task<Forum>> _createForumSetup;
    private readonly CreateForumUseCase sut;
 
    public CreateForumUseCaseShould()
    {
        var validator = new Mock<IValidator<CreateForumCommandValidator>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<CreateForumCommandValidator>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var intentionManager = new Mock<IIntentionManager>();
        intentionManager.Setup(m=>m.IsAllowed(It.IsAny<ForumIntentionType>())).Returns(true);

        _storage = new Mock<ICreateForumStorage>();
        _createForumSetup = _storage.Setup(s => s.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        sut = new CreateForumUseCase(validator.Object, intentionManager.Object, _storage.Object, new DomainMetrics(new Mock<IMeterFactory>().Object));
    }

    [Fact]
    public async Task ReturnCreateForum()
    {
        var forum = new Forum()
        {
            Id = Guid.Parse("0eb26ab4-2061-4758-8ab3-4f0f30e950cc"),
            Title = "Hi"
        };

        _createForumSetup.ReturnsAsync(forum);

        var actual = await sut.ExecuteAsync(new CreateForumCommand("Hi"), CancellationToken.None);
        actual.Should().Be(forum);
        
        _storage.Verify(s=>s.CreateAsync("Hi", It.IsAny<CancellationToken>()));
        _storage.VerifyNoOtherCalls();
    }
}