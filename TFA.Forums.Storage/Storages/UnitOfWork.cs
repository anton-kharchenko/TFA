using Microsoft.Extensions.DependencyInjection;
using TFA.Forums.Domain.Interfaces.Storages;
using TFA.Forums.Storage.Configurations;

namespace TFA.Forums.Storage.Storages;

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