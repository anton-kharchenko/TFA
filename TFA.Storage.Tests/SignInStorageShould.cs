using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TFA.Storage.Helpers;
using TFA.Storage.Storages.SignIn;
using TFA.Storage.Tests.Fixtures;

namespace TFA.Storage.Tests;

public class SignInStorageShould(SignInStorageFixture storageTestFixture) : IClassFixture<SignInStorageFixture>
{
    private readonly SignInStorage sut = new(
        storageTestFixture.GetDbContext(),
        storageTestFixture.GetMapper(),
        new GuidFactory());

    private readonly Guid userId = Guid.Parse("846090D7-0EBF-43C4-92B4-2A1577B0F15E");

    [Fact]
    public async Task ReturnUser_WhenDatabaseContainsUseWithSameLogin()
    {
        var actual = await sut.FindUserAsync("test-user", CancellationToken.None);
        actual.Should().NotBeNull();
        actual!.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task ReturnNull_WhenDatabaseNotContainsUseWithSameLogin()
    {
        var actual = await sut.FindUserAsync("whatever", CancellationToken.None);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnNewlyCreatedSessionId()
    {
        var sessionId = await sut.CreateSessionAsync(userId, new DateTimeOffset(2024, 04, 01, 15, 25, 00, TimeSpan.Zero), CancellationToken.None);

        await using var dbContext = storageTestFixture.GetDbContext();
        
        (await dbContext.Sessions.AsNoTracking().FirstAsync(s => s.SessionId == sessionId))
        .Should().NotBeNull();
    }
}