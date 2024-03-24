using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Models;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.Tests.GetForums;

public class GetForumUseCaseShould
{
    private readonly GetForumsUseCase sut;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getForumsSetup;
    private readonly Mock<IGetForumsStorage> storage;

    public GetForumUseCaseShould()
    {
        storage = new Mock<IGetForumsStorage>();

        getForumsSetup = storage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        sut = new GetForumsUseCase(storage.Object);
    }

    [Fact]
    public async Task ReturnForums_FromStorage()
    {
        var forums = new Forum[]
        {
            new() { Id = Guid.Parse("CBFD82CB-5BB3-85EE-BCBE-6860780A97B5"), Title = "Title 1" },
            new() { Id = Guid.Parse("D4A57818-F7CF-839E-9437-33A655FB12BD"), Title = "Title 2" }
        };

        getForumsSetup.ReturnsAsync(forums);

        var actual = await sut.ExecuteAsync(CancellationToken.None);

        actual.Should().BeSameAs(forums);
        storage.Verify(s => s.GetForumsAsync(CancellationToken.None), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}