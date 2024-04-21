using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Interfaces.UseCases.SignIn;
using TFA.Forum.Domain.Share;
using TFA.Forum.Storage.Configurations;
using TFA.Forum.Storage.Entities;
using TFA.Forum.Storage.Interfaces;

namespace TFA.Forum.Storage.Storages.SignIn;

public class SignInStorage(
    ForumDbContext forumDbContext,
    IMapper mapper,
    IGuidFactory guidFactory) : ISignInStorage
{
    public async Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken) =>
        await forumDbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Guid> CreateSessionAsync(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken)
    {
        var sessionId = guidFactory.Create();
        
        await forumDbContext.Sessions.AddAsync(new Session()
        {
            SessionId = sessionId,
            UserId = userId,
            ExpiresAt = expirationMoment
        }, cancellationToken);
        
        await forumDbContext.SaveChangesAsync(cancellationToken);
        
        return sessionId;
    }
}