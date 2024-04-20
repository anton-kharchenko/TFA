namespace TFA.Domain.Interfaces.Storages;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> StartScopeAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWorkScope : IAsyncDisposable
{
    TStorage GetStorage<TStorage>();
    
    Task CommitAsync(CancellationToken cancellationToken);
}

public interface IStorage;