using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase sut;
    private readonly ForumDbContext _forumDbContext;
    private readonly ISetup<IGuidFactory, Guid> _createIdSetup;
    private readonly ISetup<IMomentProvider, DateTimeOffset> _getNowSetup;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ForumDbContext>().UseInMemoryDatabase(nameof(CreateTopicUseCaseShould)).Options;
        _forumDbContext = new ForumDbContext(dbContextOptions);
        
        var guidFactory = new Mock<IGuidFactory>();
        _createIdSetup = guidFactory.Setup(f => f.Create());
        
        var momentProvider = new Mock<IMomentProvider>();
         _getNowSetup = momentProvider.Setup(p => p.Now);
        
        _createIdSetup = guidFactory.Setup(f=>f.Create());
        sut = new CreateTopicUseCase(guidFactory.Object, momentProvider.Object, _forumDbContext);
    }
    
    [Fact]
    public async Task ThrowFoundNotFoundException_WhenNoMatchingForum()
    {
        await _forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = Guid.Parse("17a42360-cd40-40de-8c54-176203da8432"),
            Title = "Basic Forum"
        });

        await _forumDbContext.SaveChangesAsync();
        
        var forumId = Guid.Parse("00749cdb-b557-4c3d-952d-a72797edd996");
        var authorId = Guid.Parse("774d3a63-3938-469b-8406-9cd9ec8fe7be");
        
        await sut.Invoking(s => s.ExecuteAsync(forumId, "title", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewlyCreatedTopic()
    {
        var forumId = Guid.Parse("c789d8ae-ebe7-4f1b-a2d8-c49ffb7b641c");
        
        await _forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = forumId,
            Title = "Existing Forum"
        });

        var userId = Guid.Parse("872d2338-64a9-4161-b403-0d72cbe984b4");

        const string donnaMiranda = "DonnaMiranda";
        
        await _forumDbContext.Users.AddAsync(new User
        {
            UserId = userId,
            Login = donnaMiranda
        });

        await _forumDbContext.SaveChangesAsync();
        _createIdSetup.Returns(Guid.Parse("44d8b0fe-652e-4986-a943-7508703ab945"));
        _getNowSetup.Returns(new DateTimeOffset(2024, 01, 30, 16, 26, 00, TimeSpan.FromHours(2)));

        const string title = "Hello world";
        
        var actual = await sut.ExecuteAsync(forumId, title, userId, CancellationToken.None);

        var topics =  await _forumDbContext.Topics.ToArrayAsync();

        topics.Should().BeEquivalentTo(new []
        {
            new Topic
            {
                ForumId = forumId,
                UserId = userId,
                Title = title
            }
        }, cfg=>cfg.Including(t=>t.ForumId).Including(t=>t.UserId).Including(t=>t.Title));

        actual.Should().BeEquivalentTo(new Models.Topic()
        {
            Id = Guid.Parse("44d8b0fe-652e-4986-a943-7508703ab945"),
            Title = title,
            Author = donnaMiranda,
            CreatedAt = new DateTimeOffset(2024, 01, 30, 16, 26, 00, TimeSpan.FromHours(2))
        });
    }
}