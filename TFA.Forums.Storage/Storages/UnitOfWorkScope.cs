using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Domain.Interfaces.Storages;

namespace TFA.Forums.Storage.Storages;

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