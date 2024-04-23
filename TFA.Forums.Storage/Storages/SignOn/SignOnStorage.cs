using TFA.Forums.Domain.Interfaces.UseCases.SignOn;
using TFA.Forums.Storage.Helpers;
using TFA.Forums.Storage.Configurations;
using TFA.Forums.Storage.Entities;
using TFA.Forums.Storage.Interfaces;

namespace TFA.Forums.Storage.Storages.SignOn;

internal class SignOnStorage(ForumDbContext forumDbContext, IGuidFactory guidFactory) : ISignOnStorage
{
    public async Task<Guid> CreateUserAsync(string login, byte[] salt, byte[] hash, CancellationToken token)
    {
        var userId = guidFactory.Create();
        await forumDbContext.Users.AddAsync(new User
        {
            UserId = userId,
            Login = login,
            Salt = salt,
            PasswordHash = hash,
        }, token);
        
        await forumDbContext.SaveChangesAsync(token);

        return userId;
    }
}