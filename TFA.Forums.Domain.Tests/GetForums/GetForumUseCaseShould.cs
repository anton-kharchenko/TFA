using Moq;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Queries.GetForum;
using TFA.Forums.Domain.UseCases.GetForums;

namespace TFA.Forums.Domain.Tests.GetForums;

public class GetForumUseCaseShould
{
    private readonly GetForumsUseCase sut;

    public GetForumUseCaseShould()
    {
        Mock<IGetForumsStorage> storage = new();

        storage.Setup(s => s.GetForumsAsync(It.IsAny<CancellationToken>()));

        sut = new GetForumsUseCase(storage.Object);
    }

    [Fact]
    public async Task ReturnForums_FromStorage()
    {
        await sut.Handle(new GetForumQuery(), CancellationToken.None);
    }
}