namespace TFA.Domain.Interfaces.Storages;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> StartScopeAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWorkScope : IAsyncDisposable
{
    TStorage GetStorage<TStorage>() where TStorage : IStorage;
    
    Task CommitAsync(CancellationToken cancellationToken);
}

public interface IStorage;