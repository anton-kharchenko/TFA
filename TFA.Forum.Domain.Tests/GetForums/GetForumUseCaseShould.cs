﻿using Moq;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Queries.GetForum;
using TFA.Forum.Domain.UseCases.GetForums;

namespace TFA.Forum.Domain.Tests.GetForums;

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