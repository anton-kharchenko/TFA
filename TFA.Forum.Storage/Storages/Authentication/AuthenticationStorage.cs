using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Authentication;
using TFA.Forum.Domain.Interfaces.Storages;
using TFA.Forum.Storage.Configurations;

namespace TFA.Forum.Storage.Storages.Authentication;

internal class AuthenticationStorage(ForumDbContext dbContext, IMapper mapper) : IAuthenticationStorage
{
    public async Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken) =>
        await dbContext.Sessions.Where(s => s.UserId == sessionId)
            .ProjectTo<Session>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
}