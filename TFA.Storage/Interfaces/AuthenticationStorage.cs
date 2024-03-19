using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Authentication;
using TFA.Domain.Interfaces.Authentication;
using TFA.Domain.Share;
using TFA.Storage.Configurations;

namespace TFA.Storage.Interfaces;

internal class AuthenticationStorage(ForumDbContext dbContext, IMapper mapper) : IAuthenticationStorage
{
    public async Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken) => await dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}