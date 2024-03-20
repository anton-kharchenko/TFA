using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TFA.Domain.Interfaces.UseCases.SignIn;
using TFA.Domain.Share;
using TFA.Storage.Configurations;

namespace TFA.Storage.Storages.SignIn;

public class SignInStorage(ForumDbContext forumDbContext, IMapper mapper) : ISignInStorage
{
    public async Task<RecognisedUser?> FindUserAsync(string login, CancellationToken cancellationToken) =>
        await forumDbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}