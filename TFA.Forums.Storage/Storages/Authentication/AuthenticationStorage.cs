using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Forums.Domain.Authentication;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Storage.Configurations;

namespace TFA.Forums.Storage.Storages.Authentication;

internal class AuthenticationStorage(ForumDbContext dbContext, IMapper mapper) : IAuthenticationStorage
{
    public async Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken) =>
        await dbContext.Sessions.Where(s => s.UserId == sessionId)
            .ProjectTo<Session>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
}