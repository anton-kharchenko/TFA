using System.Diagnostics.Metrics;
using Moq;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Monitoring;
using TFA.Domain.Queries.GetForum;
using TFA.Domain.UseCases.GetForums;

namespace TFA.Domain.Tests.GetForums;

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
    public async Task ReturnForums_FromStorage() => await sut.Handle(new GetForumQuery(),CancellationToken.None);
}