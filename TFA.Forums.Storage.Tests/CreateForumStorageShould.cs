using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Storage.Helpers;
using TFA.Forums.Storage.Storages.Forum;
using TFA.Forums.Storage.Tests.Fixtures;

namespace TFA.Forums.Storage.Tests;

public class CreateForumStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly CreateForumStorage sut = new(new GuidFactory(), fixture.GetDbContext(), fixture.GetMemoryCache(), fixture.GetMapper());

    [Fact]
    public async Task InsertNewForumInDatabase()
    {
        var forum = await sut.CreateAsync("Test title", CancellationToken.None);
        forum.Id.Should().NotBeEmpty();

        await using var dbContext = fixture.GetDbContext();
        
        var forumTitles = await dbContext.Forums.Where(f => f.ForumId == forum.Id).Select(f=>f.Title).ToArrayAsync();

        forumTitles.Should().HaveCount(1).And.Contain("Test title");
    }
}