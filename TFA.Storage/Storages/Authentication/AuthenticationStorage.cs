using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Authentication;
using TFA.Domain.Share;
using TFA.Storage.Configurations;
using IAuthenticationStorage = TFA.Domain.Interfaces.Authentication.IAuthenticationStorage;

namespace TFA.Storage.Storages.Authentication;

internal class AuthenticationStorage(ForumDbContext dbContext, IMapper mapper) : IAuthenticationStorage
{
    public async Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}