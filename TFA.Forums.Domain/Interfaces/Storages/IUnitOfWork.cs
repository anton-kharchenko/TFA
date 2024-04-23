namespace TFA.Forums.Domain.Interfaces.Storages;

public interface IUnitOfWork
{
    Task<IUnitOfWorkScope> StartScopeAsync(CancellationToken cancellationToken);
}