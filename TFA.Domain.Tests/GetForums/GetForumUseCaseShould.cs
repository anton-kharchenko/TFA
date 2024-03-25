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
        await sut.ExecuteAsync(CancellationToken.None);
    }
}