using TFA.Forum.Domain.Interfaces.UseCases.SignOn;
using TFA.Forum.Storage.Configurations;
using TFA.Forum.Storage.Entities;
using TFA.Forum.Storage.Interfaces;
using TFA.Forum.Storage.Helpers;

namespace TFA.Forum.Storage.Storages.SignOn;

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