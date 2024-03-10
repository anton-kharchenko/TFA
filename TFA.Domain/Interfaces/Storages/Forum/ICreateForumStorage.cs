namespace TFA.Domain.Interfaces.Storages.Forum;

public interface ICreateForumStorage
{
    public Task<Models.Forum> Create(string title, CancellationToken cancellationToken);
}