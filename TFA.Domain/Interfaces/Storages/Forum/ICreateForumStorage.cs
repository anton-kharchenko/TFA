namespace TFA.Domain.Interfaces.Storages.Forum;

public interface ICreateForumStorage
{
    public Task<Models.Forum> CreateAsync(string title, CancellationToken cancellationToken);
}