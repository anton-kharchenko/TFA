using AutoMapper;
using TFA.Domain.Authentication;
using TFA.Domain.Interfaces.Storages;
using TFA.Storage.Configurations;

namespace TFA.Storage.Storages.Authentication;

internal class AuthenticationStorage(ForumDbContext dbContext, IMapper mapper) : IAuthenticationStorage
{
    public Task<Session?> FindSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}