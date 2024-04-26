using FluentAssertions;
using TFA.Forums.Storage.Entities;
using TFA.Forums.Storage.Helpers;
using TFA.Forums.Storage.Storages.Comment;
using TFA.Forums.Storage.Tests.Fixtures;

namespace TFA.Forums.Storage.Tests;

public class CreateCommentStorageShould(StorageTestFixture fixture) : IClassFixture<StorageTestFixture>
{
    private readonly CreateCommentStorage _sut = new(fixture.GetDbContext(),
        new GuidFactory(),
        new MomentProvider(),
        fixture.GetMapper());

    [Fact]
    public async Task ReturnNullForTopic_WhenNoMatchingTopicInDb()
    {
        var topicId = Guid.Parse("adfc96e5-d5e3-423c-b93b-9161255c05aa");

        var actual = await _sut.FindTopicAsync(topicId, CancellationToken.None);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnFoundTopic_WhenTopicIsPresentInDb()
    {
        var topicId = Guid.Parse("f0de7180-111c-4daa-bfbf-ffcc8b7313c3");
        var userId = Guid.Parse("5813518c-198a-458f-8191-99392a66118e");
        var forumId = Guid.Parse("30106f48-6837-460f-a991-b176939fe656");
        
        const string title = "Test title";
        
        await using var dbContext = fixture.GetDbContext();
        await dbContext.Topics.AddAsync(new Topic
        {
            TopicId = topicId,
            Title = title,
            Forum = new Forum
            {
                ForumId = Guid.Parse("30106f48-6837-460f-a991-b176939fe656"),
                Title = title
            },
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 0, TimeSpan.Zero),
            Author = new User
            {
                Login = "TestUser",
                UserId = userId,
                Salt = [],
                PasswordHash = []
            }
        });
        
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var actual = await _sut.FindTopicAsync(topicId, CancellationToken.None);
        actual.Should().BeEquivalentTo(new Domain.Models.Topic()
        {
            Id = topicId,
            UserId = userId,
            ForumId = forumId,
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 00, TimeSpan.Zero),
            Title = title
        });
    }

    [Fact]
    public async Task ReturnNewlyCreatedComment_WhenCreatingComment()
    {
        var topicId = Guid.Parse("48e15cf2-4df3-4c36-83c6-3fb0e6eda074");
        var userId = Guid.Parse("00df3596-0522-4a5c-910f-b70c0ecf5e20");
        var forumId = Guid.Parse("fbd8b21b-cbbd-4477-8724-69e0b71bb70c");
        
        const string title = "Test title";
        
        await using var dbContext = fixture.GetDbContext();
        await dbContext.Topics.AddAsync(new Topic
        {
            TopicId = topicId,
            Title = title,
            Forum = new Forum
            {
                ForumId = Guid.Parse("30106f48-6837-460f-a991-b176939fe656"),
                Title = title
            },
            CreatedAt = new DateTimeOffset(2024, 04, 25, 18, 40, 00, TimeSpan.Zero),
            Author = new User
            {
                Login = "TestUser",
                UserId = userId,
                Salt = [],
                PasswordHash = []
            }
        });
        
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var comment = await _sut.CreateAsync(topicId, userId, title, CancellationToken.None);
        comment.Should().BeEquivalentTo(new Comment()
        {
            Text = title,
        }, cfg=>
            cfg.Excluding(c=>c.Id)
                .Excluding(c=>c.CreatedAt));
    }
}