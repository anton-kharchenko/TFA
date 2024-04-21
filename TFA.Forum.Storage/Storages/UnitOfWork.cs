using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forum.Domain.Interfaces.Storages;
using TFA.Forum.Storage.Configurations;

namespace TFA.Forum.Storage.Storages;

internal class UnitOfWork(IServiceProvider serviceProvider) : IUnitOfWork
{
    public async Task<IUnitOfWorkScope> StartScopeAsync(CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new UnitOfWorkScope(scope, transaction);
    }
}

internal class UnitOfWorkScope(IServiceScope scope, IDbContextTransaction transaction) : IUnitOfWorkScope
{
    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
        if (scope is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            scope.Dispose();
        }
    }

    public TStorage GetStorage<TStorage>() where TStorage : IStorage =>
        scope.ServiceProvider.GetRequiredService<TStorage>();

    public Task CommitAsync(CancellationToken cancellationToken) => 
        transaction.CommitAsync(cancellationToken);
}