namespace TFA.Domain.Interfaces.Storages;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> CreateScopeAsync();
}

public interface IUnitOfWorkScope : IAsyncDisposable
{
    TStorage GetStorage<TStorage>();
    
    Task CommitAsync(CancellationToken cancellationToken);
}

public interface IStorage;