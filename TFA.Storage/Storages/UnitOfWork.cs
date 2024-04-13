using TFA.Domain.Interfaces.Storages;

namespace TFA.Storage.Storages;

internal class UnitOfWork : IUnitOfWork
{
    public Task<IUnitOfWorkScope> CreateScopeAsync()
    {
        throw new NotImplementedException();
    }
}