using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    private readonly ForumDbContext _forumDbContext;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ForumDbContext>().UseInMemoryDatabase(nameof(CreateTopicUseCaseShould)).Options;
        _forumDbContext = new ForumDbContext(dbContextOptions);
        sut = new CreateTopicUseCase(_forumDbContext);
    }

    [Fact]
    public void ThrowFoundNotFoundException_WhenNotFound()
    {
        var forumId = Guid.Parse("00749cdb-b557-4c3d-952d-a72797edd996");
        var authorId = Guid.Parse("774d3a63-3938-469b-8406-9cd9ec8fe7be");
        sut.Invoking(s => s.ExecuteAsync(forumId, "title", authorId, CancellationToken.None))
        .Should().ThrowAsync<ForumNotFoundException>();
        
    }
}