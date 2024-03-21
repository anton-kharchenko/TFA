using TFA.Domain.Interfaces.UseCases.SignOn;
using TFA.Storage.Configurations;
using TFA.Storage.Helpers;
using TFA.Storage.Models;

namespace TFA.Storage.Storages.SignOn;

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