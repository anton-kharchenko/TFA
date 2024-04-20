using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Share;
using TFA.Storage.Configurations;
using TFA.Storage.Entities;
using TFA.Storage.Interfaces;

namespace TFA.Storage.Storages.SignIn;

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