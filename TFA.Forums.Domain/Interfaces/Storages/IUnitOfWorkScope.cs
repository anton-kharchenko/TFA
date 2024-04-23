namespace TFA.Forums.Domain.Interfaces.Storages;

public interface IUnitOfWorkScope : IAsyncDisposable
{
    TStorage GetStorage<TStorage>() where TStorage : IStorage;
    
    Task CommitAsync(CancellationToken cancellationToken);
}